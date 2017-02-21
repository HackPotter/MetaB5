using System.IO;
using UnityEditor;
using UnityEngine;

public class QuestionEditor : EditorWindow
{
    private const string kQuestionDataPath = "Assets/Resources/Questions/Database/";

    [MenuItem("Metablast/Suite/Question Editor")]
    public static void OpenItemEditor()
    {
        var window = EditorWindow.GetWindow<QuestionEditor>();
    }

    private QuestionDatabase _questionDatabase;

    private QuestionListView _listView;
    private QuestionDetailView _detailView;

    void OnEnable()
    {
        _questionDatabase = (QuestionDatabase)AssetDatabase.LoadAssetAtPath(kQuestionDataPath + "QuestionDatabase.asset", typeof(QuestionDatabase));

        if (_questionDatabase == null)
        {
            Directory.CreateDirectory(kQuestionDataPath);
            _questionDatabase = ScriptableObject.CreateInstance<QuestionDatabase>();
            AssetDatabase.CreateAsset(_questionDatabase, kQuestionDataPath + "QuestionDatabase.asset");
        }

        _listView = new QuestionListView(_questionDatabase);
        _detailView = new QuestionDetailView(_questionDatabase);

        minSize = new UnityEngine.Vector2(1000, 600);
    }

    private Rect _listViewDimensions = new Rect(0, 0, 300, 600);
    private Rect _detailViewDimensions = new Rect(301, 0, 700, 600);

    void OnGUI()
    {
        _listViewDimensions.height = this.position.height;

        _detailViewDimensions.height = this.position.height;
        _detailViewDimensions.width = this.position.width - _listViewDimensions.width;

        BeginWindows();
        GUILayout.Window(1, _listViewDimensions, _listView.OnGUI, "");
        _detailView.SelectedQuestion = _listView.SelectedItem;
        GUILayout.Window(2, _detailViewDimensions, _detailView.OnGUI, "");

        EndWindows();
    }
}
