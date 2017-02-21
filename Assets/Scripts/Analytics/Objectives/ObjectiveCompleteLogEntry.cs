using Newtonsoft.Json;
using System;

public class ObjectiveCompleteLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.ObjectiveComplete; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public ObjectiveCompleteLogEntry(Guid userGuid, GameplayObjective objective) : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(new { Name = objective.Name });
    }
}

