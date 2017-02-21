using System;
using UnityEngine;

[Serializable]
public class QuestionAnswer
{
    [SerializeField]
    private string _questionAnswer = "";

    [SerializeField]
    private string _questionFeedback = "";

    public string QuestionAnswerText
    {
        get { return _questionAnswer; }
        set { _questionAnswer = value; }
    }

    public string QuestionFeedback
    {
        get { return _questionFeedback; }
        set { _questionFeedback = value; }
    }
}

