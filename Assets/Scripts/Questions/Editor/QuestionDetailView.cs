using UnityEditor;
using UnityEngine;

public class QuestionDetailView
{
    private QuestionDatabase _questionDatabase;
    private bool _saved;

    public QuestionData SelectedQuestion
    {
        get;
        set;
    }

    public QuestionDetailView(QuestionDatabase questionDatabase)
    {
        _questionDatabase = questionDatabase;
    }

    public void OnGUI(int windowId)
    {
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(_questionDatabase);
            if (SelectedQuestion != null)
            {
                EditorUtility.SetDirty(SelectedQuestion);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.SaveAssets();
            //_saved = true;
        }


        if (SelectedQuestion == null)
        {
            GUILayout.Label("Selected question from list to view details.");
            return;
        }

        //EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Identifier:", GUILayout.ExpandWidth(false));
        SelectedQuestion.name = GUILayout.TextField(SelectedQuestion.name, GUILayout.ExpandWidth(false), GUILayout.MinWidth(50));
        GUILayout.EndHorizontal();

        GUILayout.Label("Question:");
        SelectedQuestion.QuestionText = GUILayout.TextField(SelectedQuestion.QuestionText);

        SelectedQuestion.PointValue = EditorGUILayout.IntField("Point Value", SelectedQuestion.PointValue);
        SelectedQuestion.PenaltyValue = EditorGUILayout.IntField("Incorrect Answer Penalty", SelectedQuestion.PenaltyValue);
        SelectedQuestion.PenaltyValue = Mathf.Abs(SelectedQuestion.PenaltyValue);
        SelectedQuestion.AllowNegativePoints = EditorGUILayout.Toggle("Allow Negative Points", SelectedQuestion.AllowNegativePoints, GUILayout.ExpandWidth(false));

        GUILayout.Label("Points Awarded");

        for (int i = 0; i < SelectedQuestion.QuestionAnswers.Count; i++)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("Attempt " + i + ":", GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            GUILayout.Label((Mathf.Max(SelectedQuestion.PointValue - i * SelectedQuestion.PenaltyValue, SelectedQuestion.AllowNegativePoints ? int.MinValue : 0)).ToString(), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }


        SelectedQuestion.QuestionImage = (Texture2D)EditorGUILayout.ObjectField("Supplemental Image", SelectedQuestion.QuestionImage, typeof(Texture2D), false);
        GUILayout.EndVertical();

        QuestionAnswer toRemove = null;
        foreach (QuestionAnswer answer in SelectedQuestion.QuestionAnswers)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.BeginHorizontal();
            bool isCorrect = EditorGUILayout.Toggle(SelectedQuestion.CorrectAnswerIndex == SelectedQuestion.QuestionAnswers.IndexOf(answer), GUILayout.Width(15));
            GUILayout.Label("Is Correct", GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false)))
            {
                toRemove = answer;
            }
            GUILayout.EndHorizontal();
            if (isCorrect)
            {
                SelectedQuestion.CorrectAnswerIndex = SelectedQuestion.QuestionAnswers.IndexOf(answer);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Answer:", GUILayout.ExpandWidth(false));
            answer.QuestionAnswerText = GUILayout.TextField(answer.QuestionAnswerText);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Feedback:", GUILayout.ExpandWidth(false));
            answer.QuestionFeedback = GUILayout.TextField(answer.QuestionFeedback);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Answer"))
        {
            SelectedQuestion.QuestionAnswers.Add(new QuestionAnswer());
        }

        if (toRemove != null)
        {
            int removedIndex = SelectedQuestion.QuestionAnswers.IndexOf(toRemove);
            SelectedQuestion.QuestionAnswers.Remove(toRemove);
            if (SelectedQuestion.CorrectAnswerIndex > removedIndex)
            {
                SelectedQuestion.CorrectAnswerIndex--;
            }
        }

        //_saved = !EditorGUI.EndChangeCheck();
    }
}

