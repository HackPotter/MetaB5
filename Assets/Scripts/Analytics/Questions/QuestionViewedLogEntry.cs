using Newtonsoft.Json;
using System;

public class QuestionViewedLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.QuestionViewed; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public QuestionViewedLogEntry(Guid userGuid, QuestionData question)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(
            new
            {
                QuestionName = question.name
            });
    }
}

