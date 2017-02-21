using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class QuestionListView
{
    private const string kDEFAULT_ENTRY_NAME = "New Question";
    private readonly GUIContent[] _entryContextOptions = new GUIContent[] { new GUIContent("Create New Question"), new GUIContent("Delete") };
    private readonly GUIContent[] _panelContextOptions = new GUIContent[] { new GUIContent("Create New Question") };

    private int _windowId;
    private Vector2 _scrollPosition;
    private QuestionDatabase _questionDatabase;
    private string _newEntryName = "";

    public QuestionData SelectedItem
    {
        get;
        private set;
    }

    public QuestionListView(QuestionDatabase questionDatabase)
    {
        _questionDatabase = questionDatabase;
    }

    public void OnGUI(int windowId)
    {
        _windowId = windowId;

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

        if (GUILayout.Button("Print to File"))
        {
            using (StreamWriter writer = new StreamWriter(File.Create("QuestionOutput.txt")))
            {
                WriteQuestion(_questionDatabase.Questions.ToArray(), writer);
                //foreach (QuestionData entry in _questionDatabase.Questions)
                //{
                //    if (entry == null)
                //    {
                //        continue;
                //    }
                //    WriteQuestion(entry, writer);
                //}
            }
        }

        foreach (QuestionData entry in _questionDatabase.Questions)
        {
            if (entry == null)
            {
                continue;
            }
            DrawQuestion(entry);
        }

        GUILayout.Space(10);
        DrawCreateNewEntryButton();

        GUILayout.EndScrollView();
    }

    private void PrintPair(StreamWriter writer, string s1, string s2)
    {
        PrintQuoted(writer, s1);
        writer.Write(":");
        PrintQuoted(writer, s2);
    }

    private void PrintPair(StreamWriter writer, string s1, int s2)
    {
        PrintQuoted(writer, s1);
        writer.Write(":");
        writer.Write(s2);
        writer.WriteLine(',');
    }
    private void PrintQuoted(StreamWriter writer, string str)
    {
        writer.Write("\"" + str + "\"");
    }

    private void PrintArray(StreamWriter writer, string name, params object[] vals)
    {
        PrintQuoted(writer, name);
        writer.Write(":");
        writer.Write("[");
        bool isFirst = true;
        foreach (var o in vals)
        {
            if (!isFirst)
            {
                writer.Write(',');
            }

            PrintQuoted(writer, o.ToString());
            isFirst = false;
        }
        writer.Write(']');
    }

    private class Question
    {
        public string Name;
        public int Number;
        public string QuestionText;

        public int Value;
        public int Penalty;
        public int CorrectAnswerIndex;

        public Answer[] Answers;
    }

    private class Answer
    {
        public string Text;
        public string Feedback;
    }

    public void WriteQuestion(QuestionData[] data, StreamWriter writer)
    {
        //writer.WriteLine("{");
        //string name = new string(data.name.Where((c) => !char.IsNumber(c)).ToArray());

        Question[] toWrite = new Question[data.Length];
        int index = 0;
        foreach (QuestionData q in data)
        {
            Question question = new Question();
            question.Name = new string(q.name.Where((c) => !char.IsNumber(c)).ToArray());
            question.Number = int.Parse(q.name.Substring(question.Name.Length));
            question.QuestionText = q.QuestionText;
            question.Value = q.PointValue;
            question.Penalty = q.PenaltyValue;
            question.Answers = new Answer[q.QuestionAnswers.Count];
            question.CorrectAnswerIndex = q.CorrectAnswerIndex;
            for (int i = 0; i < question.Answers.Length; i++)
            {
                question.Answers[i] = new Answer();
                question.Answers[i].Text = q.QuestionAnswers[i].QuestionAnswerText;
                question.Answers[i].Feedback = q.QuestionAnswers[i].QuestionFeedback;
            }

            toWrite[index++] = question;
        }

        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
        };
        JsonSerializer.Create(settings).Serialize(writer, toWrite);


        //writer.Write("\t");
        //PrintPair(writer, "Name", data.name);
        //writer.WriteLine(',');
        //PrintPair(writer, "Number", questionIdCounter);
        //questionIdCounter++;

        //PrintPair(writer, "QuestionText", data.QuestionText);
        //PrintPair(writer, "Points", data.PointValue);
        //PrintPair(writer, "Penalty", data.PenaltyValue);
        //PrintPair(writer, "CorrectAnswer", data.CorrectAnswerIndex);


        //PrintArray(writer, "Answers", data.QuestionAnswers.ToArray());


        //writer.WriteLine("Question: " + data.name);
        //writer.WriteLine("\tText: " + data.QuestionText);
        //writer.WriteLine("\tCorrect Answer: " + data.CorrectAnswerIndex);
        //writer.WriteLine("\tValue: " + data.PointValue);
        //writer.WriteLine("\tIncorrect Answer Penalty: " + data.PenaltyValue);

        //int counter = 0;
        //writer.Write("\tAnswers:");
        //writer.WriteLine('[');
        //foreach (var answer in data.QuestionAnswers)
        //{
        //    writer.WriteLine('{');
        //    writer.WriteLine("\t\t" + answer.QuestionAnswerText);
        //    writer.WriteLine("\t\tFeedback: " + answer.QuestionFeedback);

        //    writer.WriteLine('}');
        //    counter++;
        //}
        //writer.WriteLine();
    }

    private void DrawQuestion(QuestionData entry)
    {
        if (GUILayout.Button(entry.name))
        {
            if (Event.current.button == 0)
            {
                SelectedItem = entry;
            }
            else if (Event.current.button == 1)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _entryContextOptions, -1, ContextMenuCallback, entry);
            }
            GUI.FocusWindow(_windowId);
        }
    }

    private void CheckPanelContextMenu()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _panelContextOptions, -1, PanelContextMenuCallback, null);
            Event.current.Use();
        }
    }

    private void PanelContextMenuCallback(object obj, string[] items, int selection)
    {
        switch (selection)
        {
            case 0:
                CreateNewEntry("");
                break;
        }
    }

    private void DrawCreateNewEntryButton()
    {
        GUI.SetNextControlName("QuestionListView.QuestionTextField");
        _newEntryName = GUILayout.TextField(_newEntryName);
        if (GUILayout.Button("Create New Question") || GUI.GetNameOfFocusedControl() == "QuestionListView.QuestionTextField" && Event.current.isKey && Event.current.keyCode == KeyCode.Return)
        {
            CreateNewEntry(_newEntryName);
            _newEntryName = "";
            Event.current.Use();
        }
    }

    private void ContextMenuCallback(object obj, string[] items, int selection)
    {
        QuestionData entry = obj as QuestionData;
        switch (selection)
        {
            case 0:
                CreateNewEntry("");
                break;
            case 1:
                if (entry != null)
                {
                    _questionDatabase.Questions.Remove(entry);
                    UnityEngine.Object.DestroyImmediate(entry, true);
                    EditorUtility.SetDirty(_questionDatabase);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.SaveAssets();
                    SelectedItem = null;
                }
                break;
        }
    }

    private void CreateNewEntry(string name)
    {
        if (name == "")
        {
            name = kDEFAULT_ENTRY_NAME;
        }
        QuestionData item = ScriptableObject.CreateInstance<QuestionData>();
        item.name = name;
        _questionDatabase.Questions.Add(item);

        AssetDatabase.AddObjectToAsset(item, _questionDatabase);
        EditorUtility.SetDirty(item);
        EditorUtility.SetDirty(_questionDatabase);
        AssetDatabase.SaveAssets();
        AssetDatabase.SaveAssets();
    }
}