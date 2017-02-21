using Newtonsoft.Json;
using System;

public class QuestionAnsweredLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.QuestionAnswered; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public QuestionAnsweredLogEntry(Guid userGuid, QuestionProgress question, QuestionAnswer questionAnswer)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(
            new
            {
                QuestionName = question.QuestionData.name,
                QuestionAnswer = new
                {
                    CorrectAnswerIndex = question.QuestionData.CorrectAnswerIndex,
                    AnswerIndex = question.QuestionData.QuestionAnswers.IndexOf(questionAnswer),
                    AnswerText = questionAnswer.QuestionAnswerText,
                }
            });
    }
}

