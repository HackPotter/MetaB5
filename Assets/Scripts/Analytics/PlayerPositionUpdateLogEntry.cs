using Newtonsoft.Json;
using System;
using UnityEngine;

public class PlayerPositionUpdateLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.PlayerPositionUpdate; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public PlayerPositionUpdateLogEntry(Guid userGuid, Vector3 position) : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(new { X = position.x, Y = position.y, Z = position.z });
    }
}

