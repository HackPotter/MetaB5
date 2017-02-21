using Newtonsoft.Json;
using System;

public class BiologEntryUnlockedLogEntry : AnalyticsLogEntry
{
    private string _data = "";

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.BiologEntryUnlocked; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public BiologEntryUnlockedLogEntry(Guid userGuid, BiologEntry entry)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(
            new
            {
                BiologEntry = entry.EntryName
            });
    }
}
