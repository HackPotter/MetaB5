using Newtonsoft.Json;
using System;

public class LightToolEnabledLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.LightToolActive; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public LightToolEnabledLogEntry(Guid userGuid, bool lightEnabled)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(
            new
            {
                LightEnabled = lightEnabled
            });
    }
}
