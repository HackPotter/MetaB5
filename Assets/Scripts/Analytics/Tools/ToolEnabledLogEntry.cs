using Newtonsoft.Json;
using System;

public class ToolEnabledLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.ToolEnabled; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public ToolEnabledLogEntry(Guid userGuid, ActiveTool tool)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(new { Tool = tool.ToString() });
    }
}

