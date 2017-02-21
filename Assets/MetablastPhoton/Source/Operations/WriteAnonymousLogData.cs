using System;
using System.Collections.Generic;
using MetablastServer.Codes;

public class WriteAnonymousLogData : BaseRequest
{
    private Guid _userGuid;
    private float _gameTime;
    private LogEntryType _type;
    private string _data;

    public WriteAnonymousLogData(Guid userGuid, float gameTime, LogEntryType entryType, string data)
    {
        _userGuid = userGuid;
        _gameTime = gameTime;
        _type = entryType;
        _data = data;
    }

    public override bool IsReliable
    {
        get { return true; }
    }

    public override bool Encrypt
    {
        get { return false; }
    }

    public override byte ChannelId
    {
        get { return 0; }
    }

    protected override RequestOpCode RequestOpCode
    {
        get { return RequestOpCode.WriteAnonymousLogEntry; }
    }

    protected override void PopulateParameters(Dictionary<byte, object> parameters)
    {
        parameters[(byte)WriteAnonymousLogEntryParameters.UserGuid] = Convert.ToBase64String(_userGuid.ToByteArray());
        parameters[(byte)WriteAnonymousLogEntryParameters.GameTime] = _gameTime;
        parameters[(byte)WriteAnonymousLogEntryParameters.EntryType] = (int)_type;
        parameters[(byte)WriteAnonymousLogEntryParameters.Data] = _data;
    }
}

