#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
using UnityEditor;
using UnityEngine;

public class TagSearcher : EditorWindow
{
    static TagSearcher window;
    static string tagValue = "";
    static string oldTagValue;
    static Vector2 scrollValue = Vector2.zero;
    static GameObject[] searchResult;

    [MenuItem("EditorUtility/TagSearcher")]
    static void OpenTagSearcher()
    {
        window = (TagSearcher)EditorWindow.GetWindow(typeof(TagSearcher));
        searchResult = GameObject.FindGameObjectsWithTag(tagValue);
    }

    void OnGUI()
    {
        oldTagValue = tagValue;
        tagValue = EditorGUILayout.TagField(tagValue);

        if (tagValue != oldTagValue)
        {
            searchResult = GameObject.FindGameObjectsWithTag(tagValue);
            Selection.objects = searchResult;
        }

        scrollValue = EditorGUILayout.BeginScrollView(scrollValue);
        if (searchResult != null)
        {
            if (GUILayout.Button("Select All"))
            {
                Selection.objects = searchResult;
            }

            foreach (GameObject obj in searchResult)
            {
                if (obj != null)
                {
                    if (GUILayout.Button(obj.name))//,GUIStyle.none))
                    {
                        Selection.activeObject = obj;
                        EditorGUIUtility.PingObject(obj);
                    }
                }
                else
                {
                    searchResult = GameObject.FindGameObjectsWithTag(tagValue);
                    Selection.objects = searchResult;
                    break;
                }
            }

        }

        EditorGUILayout.EndScrollView();
    }

}