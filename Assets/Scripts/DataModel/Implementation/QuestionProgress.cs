using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionProgress
{
    private QuestionData _questionData;

    private List<QuestionAnswer> _chosenAnswers = new List<QuestionAnswer>();

    public QuestionData QuestionData
    {
        get { return _questionData; }
    }

    public bool Completed
    {
        get { return _chosenAnswers.Contains(_questionData.QuestionAnswers[_questionData.CorrectAnswerIndex]); }
    }

    public QuestionAnswer CorrectAnswer
    {
        get { return _questionData.QuestionAnswers[_questionData.CorrectAnswerIndex]; }
    }

    public QuestionProgress(QuestionData questionData)
    {
        _questionData = questionData;
    }

    public void ChooseAnswer(QuestionAnswer answer)
    {
        if (!_chosenAnswers.Contains(answer))
        {
            _chosenAnswers.Add(answer);
            AnalyticsLogger.Instance.AddLogEntry(new QuestionAnsweredLogEntry(GameContext.Instance.Player.UserGuid, this, answer));
        }
    }

    public bool AnswerChosen(QuestionAnswer answer)
    {
        return _chosenAnswers.Contains(answer);
    }

    public int CalculatePoints()
    {
        int incorrectAnswers = _chosenAnswers.Count((answer) => _questionData.QuestionAnswers.IndexOf(answer) != _questionData.CorrectAnswerIndex);

        int penalizedValue = _questionData.PointValue - incorrectAnswers * _questionData.PenaltyValue;
        penalizedValue = _questionData.AllowNegativePoints ? penalizedValue : Mathf.Max(penalizedValue, 0);

        return penalizedValue;
    }
}

public class PlayerQuestionProgress
{
    private Dictionary<QuestionData, QuestionProgress> _questionProgress = new Dictionary<QuestionData, QuestionProgress>();
    
    public QuestionProgress GetQuestionProgress(QuestionData questionData)
    {
        QuestionProgress questionProgress;
        if (!_questionProgress.TryGetValue(questionData, out questionProgress))
        {
            questionProgress = new QuestionProgress(questionData);
            _questionProgress.Add(questionData, questionProgress);
        }
        return questionProgress;
    }
}

