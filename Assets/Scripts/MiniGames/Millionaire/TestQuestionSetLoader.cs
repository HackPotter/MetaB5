using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(QuestionSetLoader))]
public class TestQuestionSetLoader : MonoBehaviour
{
    void Awake()
    {
        QuestionSetLoader loader = GetComponent<QuestionSetLoader>();

        string message = "";
        foreach (var questionSet in loader.QuestionSets)
        {
            message += "Question Set: " + questionSet.Title + "\n";
            message += "Category: " + questionSet.Category + "\n";
            message += "Levels: " + questionSet.Levels.Length + "\n";
            foreach (var level in questionSet.Levels)
            {
                message += "Level:" + "\n";
                message += "    Questions: " + level.questions.Length + "\n";
                foreach (var question in level.questions)
                {
                    message += "        Text: " + question.content + "\n";
                    message += "        ImgPath: " + question.imagePath + "\n";
                    message += "        Answers: " + question.answers.Length + "\n";
                    foreach (var answer in question.answers)
                    {
                        message += "            Text: " + answer.content + " - Correct: " + answer.correct + "\n";
                    }
                }
            }
            message += "\n\n";
        }

        Debug.Log(message);
    }
}

