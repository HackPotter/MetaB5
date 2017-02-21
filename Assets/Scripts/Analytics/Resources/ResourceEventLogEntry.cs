using Newtonsoft.Json;
using System;

public class ResourceEventLogEntry : AnalyticsLogEntry
{
    public enum ResourceType
    {
        ATP, NADPH, O2
    }

    public enum EventType
    {
        Full,
        Empty,
    }
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.ResourceEvent; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public ResourceEventLogEntry(Guid userGuid, ResourceType resourceType, EventType eventType)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(
            new
            {
                ResourceType = resourceType.ToString(),
                EventType = eventType.ToString()
            });
    }
}

