using System;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(QuestionSetLoader))]

public class TestQuestionSetLoader : MonoBehaviour {

    void Awake() {
        try {
            QuestionSetLoader loader = GetComponent<QuestionSetLoader>();

            string message = "";
            foreach (var questionSet in loader.QuestionSets) {
                message += "Question Set: " + questionSet.Title + "\n";
                message += "Category: " + questionSet.Category + "\n";
                message += "Levels: " + questionSet.Levels.Length + "\n";
                if (questionSet.Levels == null) {
                    UnityEngine.Debug.Log("Questionset.levels is null");
                }

                foreach (var level in questionSet.Levels) {
                    if (level == null) {
                        UnityEngine.Debug.Log(questionSet.Title + ": level is null");
                    }
                }
                message += "\n\n";
            }

            UnityEngine.Debug.Log(message);
        }
        catch (Exception ex) {
            StackFrame callStack = new StackFrame(1, true);
            UnityEngine.Debug.Log("Error: " + ex.InnerException.Message + ", File: " + callStack.GetFileName() + ", Line: " + callStack.GetFileLineNumber());
        }
    }
}

