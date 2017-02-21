using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using MetablastServer.Codes;
using UnityEngine;

public delegate void ConnectionStatusChangedHandler(StatusCode statusCode);


public class NetworkManager : MonoBehaviour, IPhotonPeerListener
{
    private PhotonPeer _peer;
    private static NetworkManager _instance;
    public event ConnectionStatusChangedHandler ConnectionStatusChanged;
    public event ConnectionStatusChangedHandler Connected;
    public event ConnectionStatusChangedHandler Disconnected;
    public event ConnectionStatusChangedHandler EncryptionEstablished;
    public event ConnectionStatusChangedHandler EncryptionFailed;


    Dictionary<RequestOpCode, List<IResponseHandler>> _handlersByOpCode = new Dictionary<RequestOpCode,List<IResponseHandler>>();
    Dictionary<Type, IResponseHandler> _handlersByType = new Dictionary<Type,IResponseHandler>();

    public T GetHandler<T>() where T: class, IResponseHandler, new()
    {
        IResponseHandler handler;
        if(_handlersByType.TryGetValue(typeof(T), out handler))
        {
            return handler as T;
        }
        
        handler = new T();
        if(!_handlersByOpCode.ContainsKey(handler.OperationCode))
        {
            _handlersByOpCode[handler.OperationCode] = new List<IResponseHandler>();
        }
        _handlersByOpCode[handler.OperationCode].Add(handler);
        return handler as T;
    }

    public PeerStateValue ConnectionStatus
    {
        get { return _peer.PeerState; }
    }

    private void Awake()
    {
        _peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        _peer.Service();
    }

    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject networkManagerGameObject = new GameObject("NetworkManager");
                
                _instance = networkManagerGameObject.AddComponent<NetworkManager>();
            }
            return _instance;
        }
    }

    public bool Connect(string serverAddress, string applicationName)
    {
        return _peer.Connect(serverAddress, applicationName);
    }

    public bool EstablishEncryption()
    {
        return _peer.EstablishEncryption();
    }

    public bool SendRequest(BaseRequest request)
    {
        return _peer.OpCustom(request.Build(), request.IsReliable, request.ChannelId, request.Encrypt);
    }

    public void Disconnect()
    {
        _peer.Disconnect();
    }

    void OnApplicationQuit()
    {
        if (ConnectionStatus != PeerStateValue.Disconnected && ConnectionStatus != PeerStateValue.Disconnecting)
        {
            _peer.Disconnect();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnEvent(EventData eventData)
    {
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        if (!_handlersByOpCode.ContainsKey((RequestOpCode)operationResponse.OperationCode))
            return;
        foreach (IResponseHandler handler in _handlersByOpCode[(RequestOpCode)operationResponse.OperationCode])
        {
            handler.HandleResponse(operationResponse);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                if (Connected != null)
                {
                    Connected(statusCode);
                }
                break;
            case StatusCode.Disconnect:
            case StatusCode.DisconnectByServer:
            case StatusCode.DisconnectByServerLogic:
            case StatusCode.DisconnectByServerUserLimit:
            case StatusCode.TimeoutDisconnect:
                if (Disconnected != null)
                    Disconnected(statusCode);
                break;
            case StatusCode.EncryptionEstablished:
                if (EncryptionEstablished != null)
                {
                    EncryptionEstablished(statusCode);
                }
                break;
            case StatusCode.EncryptionFailedToEstablish:
                if (EncryptionFailed != null)
                {
                    EncryptionFailed(statusCode);
                }
                break;
        }
        
        if (ConnectionStatusChanged != null)
        {
            ConnectionStatusChanged(statusCode);
        }
    }
}
