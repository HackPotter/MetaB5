#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BiologEditor : EditorWindow
{
    private const string kBIOLOG_DATA_PATH = "Assets/Resources/Biolog/Database/BiologData_Converted.asset";
    [MenuItem("Metablast/Suite/Biolog Editor")]
    public static void OpenBiologEditor()
    {
        var window = EditorWindow.GetWindow<BiologEditor>();
    }

    private BiologEntryDetailView _detailView;
    private BiologEntryListView _listView;
    private BiologEditorModel _model;
    private BiologData _data;


    void OnEnable()
    {
        _data = (BiologData)AssetDatabase.LoadAssetAtPath(kBIOLOG_DATA_PATH, typeof(BiologData));
        if (_data == null)
        {
            _data = ScriptableObject.CreateInstance<BiologData>();
            if (!Directory.Exists("Assets/Biolog"))
            {
                AssetDatabase.CreateFolder("Assets", "Biolog");
            }
            if (!Directory.Exists("Assets/Biolog/Data"))
            {
                AssetDatabase.CreateFolder("Assets/Biolog", "Data");
            }
            AssetDatabase.CreateAsset(_data, kBIOLOG_DATA_PATH);
        }

        HashSet<string> allTags = new HashSet<string>();
        foreach (var entry in _data.Entries)
        {
            foreach (string tag in entry.Tags)
            {
                if (!allTags.Contains(tag))
                {
                    allTags.Add(tag);
                }
            }
        }


        minSize = new UnityEngine.Vector2(1000, 600);
        _model = new BiologEditorModel(this, allTags);
        _detailView = new BiologEntryDetailView(_model, _data);
        _listView = new BiologEntryListView(_model, _data);
    }

    private Rect _listViewDimensions = new Rect(0,0,300,600);
    private Rect _detailViewDimensions = new Rect(301,0,700,600);

    void OnGUI()
    {
        _listViewDimensions.height = this.position.height;

        _detailViewDimensions.height = this.position.height;
        _detailViewDimensions.width = this.position.width - _listViewDimensions.width;

        BeginWindows();
        GUILayout.Window(1, _listViewDimensions, _listView.OnGUI, "");
        GUILayout.Window(2, _detailViewDimensions, _detailView.OnGUI, "");

        EndWindows();
    }
}
