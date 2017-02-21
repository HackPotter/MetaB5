using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class QuestionDatabase : ScriptableObject
{
    [SerializeField]
    private List<QuestionData> _questions = new List<QuestionData>();

    public List<QuestionData> Questions
    {
        get { return _questions; }
    }
}

