using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CleanUpWindow : EditorWindow
{
    public bool groupEnabled = false;
    private List<string> usedAssets = new List<string>();
    private List<string> includedDependencies = new List<string>();
    private Vector2 scrollPos;
    private List<string> unUsed;
    private bool needToBuild = false;

    // Add menu named "CleanUpWindow" to the Window menu  
    [MenuItem("Metablast/Utility/CleanUpWindow")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:  
        CleanUpWindow window = (CleanUpWindow)EditorWindow.GetWindow(typeof(CleanUpWindow));
        window.Show();
    }

    void OnGUI()
    {
        if (needToBuild)
        {
            GUI.color = Color.red;
            GUILayout.Label("Are you sure you remembered to build project? Because you really need to...", EditorStyles.boldLabel);
        }
        GUI.color = Color.white;

        if (GUILayout.Button("Load EditorLog"))
        {
            loadEditorLog();
        }


        if (!needToBuild)
        {
            /*
            EditorGUILayout.BeginVertical();
            if (groupEnabled)
            {
                GUILayout.Label("DEPENDENCIES");
                for (int i = 0; i < includedDependencies.Count; i++)
                {
                    EditorGUILayout.LabelField(includedDependencies[i]);
                }
            }
            EditorGUILayout.EndVertical();
             */
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginVertical();


            

            if (groupEnabled)
            {
                foreach (string asset in unUsed)
                {
                    GUILayout.Label(asset);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

    }

    private void loadEditorLog()
    {

        Debug.Log("Getting All Assets!");
        UsedAssets.GetLists(ref usedAssets, ref includedDependencies);

        if (usedAssets.Count == 0)
        {
            needToBuild = true;
        }
        else
        {
            compareAssetList(UsedAssets.GetAllAssets());
            groupEnabled = true;
            needToBuild = false;
        }
    }

    private void compareAssetList(string[] assetList)
    {
        Debug.Log("Comparing Asset Lists!");

        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UnusedDependencies.txt";
        Debug.Log("Writing Dependencies to: " + path);
        StreamWriter writer = new StreamWriter(File.Create(path));
        unUsed = new List<string>();

        for (int i = 0; i < assetList.Length; i++)
        {
            if (!usedAssets.Contains(assetList[i]))
            {
                unUsed.Add(assetList[i]);
                writer.WriteLine(assetList[i]);
            }
        }

        writer.Close();
    }
}