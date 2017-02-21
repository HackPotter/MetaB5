using Newtonsoft.Json;
using System;

public class BiologEntrySelectedLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.BiologEntrySelected; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public BiologEntrySelectedLogEntry(Guid userGuid, BiologEntry biologEntry)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(
            new
            {
                BiologEntry = biologEntry.EntryName
            });
    }
}

