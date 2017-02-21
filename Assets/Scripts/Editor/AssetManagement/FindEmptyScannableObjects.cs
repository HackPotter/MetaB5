using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FindEmptyScannableObjects : EditorWindow
{
    [MenuItem("Metablast/Utility/Find Empty Scannable Objects")]
    private static void ShowWindow()
    {
        var window = EditorWindow.GetWindow<FindEmptyScannableObjects>();
        window.Show();
        window.Focus();
    }

    private ScannableObject[] _allListeners;
    private Vector2 _scroll;

    void OnGUI()
    {
        if (GUILayout.Button("Do it"))
        {
            _allListeners = (ScannableObject[])GameObject.FindObjectsOfType(typeof(ScannableObject));
            List<ScannableObject> emptyOnes = new List<ScannableObject>();
            foreach (var v in _allListeners)
            {
                if (string.IsNullOrEmpty(v.BiologEntry))
                {
                    emptyOnes.Add(v);
                }
            }

            _allListeners = emptyOnes.ToArray();
        }

        if (GUILayout.Button("Clear"))
        {
            _allListeners = null;
        }

        if (_allListeners != null)
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            foreach (UnityEngine.Object obj in _allListeners)
            {
                EditorGUILayout.ObjectField(obj, typeof(ScannableObject), true);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
