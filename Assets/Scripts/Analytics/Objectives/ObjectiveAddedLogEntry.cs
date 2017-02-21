using Newtonsoft.Json;
using System;

public class ObjectiveAddedLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.ObjectiveAdded; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public ObjectiveAddedLogEntry(Guid userGuid, GameplayObjective objective)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(new { ObjectiveName = objective.Name });
    }
}

