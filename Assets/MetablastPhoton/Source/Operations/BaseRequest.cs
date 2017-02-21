using System.Collections.Generic;
using ExitGames.Client.Photon;
using MetablastServer.Codes;

public abstract class BaseRequest
{
    public abstract bool IsReliable { get; }
    public abstract bool Encrypt { get; }
    public abstract byte ChannelId { get; }
    protected abstract RequestOpCode RequestOpCode { get; }
    protected abstract void PopulateParameters(Dictionary<byte, object> parameters);

    public OperationRequest Build()
    {
        OperationRequest request = new OperationRequest();
        request.OperationCode = (byte)RequestOpCode;
        var p = new Dictionary<byte, object>();
        PopulateParameters(p);
        request.Parameters = p;
        return request;
    }
}

