using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnalyticsLogger
{
    private const string kLogFile = "analytics_cache.log";
    private const int kMaxConcurrent = 20;
    private const int kMaxFileSize = 1 << 25;

    private static AnalyticsLogger _instance;

    public static AnalyticsLogger Instance
    {
        get
        {
            return (_instance = _instance ?? new AnalyticsLogger());
        }
    }

    private AnalyticsLogger()
    {
        GameObject coroutineSurrogate = new GameObject("AnalyticsSurrogate");
        GameObject.DontDestroyOnLoad(coroutineSurrogate);
        CoroutineRunner runner = coroutineSurrogate.AddComponent<CoroutineRunner>();

        //runner.StartCoroutine(SendAnalytics());
    }


    private float _lastConnectionAttempt = float.MaxValue;
    private string _connectionAddress;
    private string _connectionApplication;
    private Queue<AnalyticsLogEntry> _logEntry = new Queue<AnalyticsLogEntry>();

    public void AddLogEntry(AnalyticsLogEntry logEntry)
    {
        _logEntry.Enqueue(logEntry);
    }

    public void Load()
    {
        if (File.Exists(kLogFile))
        {
            if (new FileInfo(kLogFile).Length > kMaxFileSize)
            {
                File.Delete(kLogFile);
                return;
            }
            try
            {
                using (StreamReader reader = new StreamReader(File.Open(kLogFile, FileMode.Open, FileAccess.Read)))
                {
                    string file = reader.ReadToEnd();
                    string[] lines = file.Split('\n');

                    int i = 0;
                    do
                    {
                        Guid guid = new Guid(Convert.FromBase64String(lines[i++]));
                        LogEntryType logEntryType = (LogEntryType)Enum.Parse(typeof(LogEntryType), lines[i++]);
                        float time = float.Parse(lines[i++]);
                        string data = lines[i++];

                        _logEntry.Enqueue(new AnalyticsLogEntry(guid, logEntryType, data, time));
                    }
                    while ((i + 3) < lines.Length);
                }
            }
            catch
            {
            }
            finally
            {
                File.Delete(kLogFile);
            }
        }
    }

    public void ApplicationQuit()
    {
        try
        {
            if (File.Exists(kLogFile))
            {
                File.Delete(kLogFile);
            }
            using (StreamWriter writer = new StreamWriter(File.Open(kLogFile, FileMode.Create, FileAccess.Write)))
            {
                while (_logEntry.Count > 0)
                {
                    var entry = _logEntry.Dequeue();
                    writer.Write(Convert.ToBase64String(entry.UserGuid.ToByteArray()) + "\n");
                    writer.Write(entry.LogEntryType.ToString() + "\n");
                    writer.Write(entry.EventTime + "\n");
                    writer.Write(entry.Data + "\n");
                }
            }
        }
        catch
        {
            File.Delete(kLogFile);
        }
        finally
        {
        }
    }

    private IEnumerator SendAnalytics()
    {
        while (true)
        {
            switch (NetworkManager.Instance.ConnectionStatus)
            {
                case PeerStateValue.Connected:
                    int numSent = 0;

                    if (_logEntry.Count > 0)
                    {
                        Debug.Log("Connected. " + _logEntry.Count + " entries in queue.");
                    }
                    while (_logEntry.Count > 0 && numSent < kMaxConcurrent)
                    {
                        var logEntry = _logEntry.Peek();
                        if (NetworkManager.Instance.SendRequest(new WriteAnonymousLogData(logEntry.UserGuid, logEntry.EventTime, logEntry.LogEntryType, logEntry.Data)))
                        {
                            _logEntry.Dequeue();
                        }
                        numSent++;
                    }
                    break;
                case PeerStateValue.Disconnected:
                    if (Time.time - _lastConnectionAttempt < 3f)
                    {
                        Debug.Log("Attempted to connect recently. Waiting for 3 seconds");
                        yield return new WaitForSeconds(3f);
                    }
                    WWW www = new WWW(@"http://metnet-mbl.gdcb.iastate.edu/server.txt");
                    
                    yield return www;
                    if (!string.IsNullOrEmpty(www.error))
                    {
                        yield return new WaitForSeconds(5);
                        break;
                    }
                    
                    Debug.Log("Got connection address: " + www.text);
                    string[] serverInfo = www.text.Split('\n');
                    if (serverInfo.Length != 2)
                    {
                        Debug.Log("Invalid, though");
                        yield return new WaitForSeconds(5);
                        break;
                    }
                    _lastConnectionAttempt = Time.time;
                    _connectionAddress = serverInfo[0];
                    _connectionApplication = serverInfo[1];
                    if (!NetworkManager.Instance.Connect(_connectionAddress, _connectionApplication))
                    {
                        yield return new WaitForSeconds(5);
                    }
                    break;
                case PeerStateValue.Disconnecting:
                    break;
                case PeerStateValue.InitializingApplication:
                    break;
                case PeerStateValue.Connecting:
                    break;
            }

            yield return null;
        }
    }
}
