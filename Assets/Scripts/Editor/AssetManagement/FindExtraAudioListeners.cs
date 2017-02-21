using UnityEditor;
using UnityEngine;

public class FindExtraAudioListeners : EditorWindow
{
    [MenuItem("Metablast/Utility/Find Extra Audio Listeners Util")]
    private static void ShowWindow()
    {
        var window = EditorWindow.GetWindow<FindExtraAudioListeners>();
        window.Show();
        window.Focus();
    }

    private Object[] _allListeners;

    void OnGUI()
    {
        if (GUILayout.Button("Find All Audio Listeners"))
        {
            _allListeners = GameObject.FindObjectsOfType(typeof(AudioListener));
        }

        if (GUILayout.Button("Clear"))
        {
            _allListeners = null;
        }

        if (_allListeners != null)
        {
            foreach (UnityEngine.Object obj in _allListeners)
            {
                EditorGUILayout.ObjectField(obj, typeof(AudioListener),true);
            }
        }
    }
}
