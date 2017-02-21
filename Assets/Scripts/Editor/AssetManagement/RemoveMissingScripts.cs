using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class RemoveMissingScripts : EditorWindow
{
    [MenuItem("Window/Utility/Remove Missing Scripts")]
    public static void CreateWindow()
    {
        var window = EditorWindow.CreateInstance<RemoveMissingScripts>();
        window.Show();
    }

    private Vector2 _scrollPosition;
    private List<GameObject> _prefabsWithMissingComponents;

    int _lastCount = 0;
    void OnGUI()
    {
        if (GUILayout.Button("Remove Missing Scripts!"))
        {
            object[] sceneObjects = (object[])GameObject.FindObjectsOfType(typeof(GameObject));
            _lastCount = sceneObjects.Length;
            _prefabsWithMissingComponents = new List<GameObject>();

            foreach (object obj in sceneObjects)
            {
                if (!(obj is GameObject))
                {
                    continue;
                }
                if (CheckForAndRemoveMissingBehaviours(obj as GameObject) > 0)
                {
                    _prefabsWithMissingComponents.Add(obj as GameObject);
                    
                }
            }
            /*
            foreach (string s in GetAllPrefabPaths())
            {
                GameObject prefabAsset = InstantiatePrefab(s);
                if (prefabAsset == null)
                {
                    continue;
                }
                int count = CheckForAndRemoveMissingBehaviours(prefabAsset);
                if (count > 0)
                {
                    _prefabsWithMissingComponents.Add(prefabAsset);
                }
            }*/
        }

        if (_prefabsWithMissingComponents != null)
        {
            GUILayout.Label("Found " + _lastCount + " game objects total");
            GUILayout.Label("Game Objects:");
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            foreach (GameObject prefabAsset in _prefabsWithMissingComponents)
            {
                EditorGUILayout.ObjectField(prefabAsset, typeof(GameObject), false);
            }
            GUILayout.Label("End.");
            GUILayout.EndScrollView();
        }
    }

    string[] GetAllPrefabPaths()
    {
        return Directory.GetFiles("Assets/", "*.prefab",SearchOption.AllDirectories);
    }

    GameObject InstantiatePrefab(string path)
    {
        Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        if (asset == null)
        {
            return null;
        }
        return asset as GameObject;
    }

    int CheckForAndRemoveMissingBehaviours(GameObject gameObject)
    {
        int count = 0;
        Component[] components = gameObject.GetComponents<Component>();

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null || !components[i])
            {
                count++;
            }
        }

        //foreach (Transform child in gameObject.transform)
        //{
        //    count += CheckForAndRemoveMissingBehaviours(child.gameObject);
        //}

        return count;
    }
}
