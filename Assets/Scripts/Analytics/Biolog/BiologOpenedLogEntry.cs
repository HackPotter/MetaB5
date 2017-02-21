using System;

public class BiologOpenedLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.BiologOpened; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public BiologOpenedLogEntry(Guid userGuid)
        : base(userGuid)
    {
        _data = "";
    }
}

