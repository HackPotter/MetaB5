using Newtonsoft.Json;
using System;

public class ObjectiveTaskCompleteLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.ObjectiveTaskComplete; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public ObjectiveTaskCompleteLogEntry(Guid userGuid, GameplayObjective objective, ObjectiveTask task)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(new { ObjectiveName = objective.Name, TaskName = task.Name });
    }
}

