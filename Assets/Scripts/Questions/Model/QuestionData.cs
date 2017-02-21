using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestionData : ScriptableObject
{
    [SerializeField]
    private int _pointValue;

    [SerializeField]
    private int _penaltyValue;

    [SerializeField]
    private string _question = "";

    [SerializeField]
    private List<QuestionAnswer> _questionAnswers = new List<QuestionAnswer>();

    [SerializeField]
    private int _correctAnswerIndex;

    [SerializeField]
    private bool _allowNegativePoints;

    [SerializeField]
    private Texture2D _questionImage;

    [SerializeField]
    private int _id;

    public string QuestionText
    {
        get { return _question; }
        set { _question = value; }
    }

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public List<QuestionAnswer> QuestionAnswers
    {
        get { return _questionAnswers; }
    }

    public int CorrectAnswerIndex
    {
        get { return _correctAnswerIndex; }
        set { _correctAnswerIndex = value; }
    }

    public int PointValue
    {
        get { return _pointValue; }
        set { _pointValue = value; }
    }

    public int PenaltyValue
    {
        get { return _penaltyValue; }
        set { _penaltyValue = value; }
    }

    public bool AllowNegativePoints
    {
        get { return _allowNegativePoints; }
        set { _allowNegativePoints = value; }
    }

    public Texture2D QuestionImage
    {
        get { return _questionImage; }
        set { _questionImage = value; }
    }
}

