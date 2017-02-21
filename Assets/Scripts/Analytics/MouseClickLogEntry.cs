using Newtonsoft.Json;
using System;
using UnityEngine;

public class MouseClickLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.MouseClick; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public MouseClickLogEntry(Guid userGuid, MouseButton button, Vector2 position)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(
            new
            {
                MouseButton = (int)button,
                Position = new
                {
                    x = position.x,
                    y = position.y
                }
            });
    }
}
