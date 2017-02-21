using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text.RegularExpressions;

public class InspectorBase<T> : Editor where T : UnityEngine.Object
{
    protected T Target { get { return (T)target; } }
}

[CustomEditor(typeof(RailNode))]
public class RailNodeEditor : InspectorBase<RailNode>
{
    //Add "Select next"
    //Add "Select prev"
    //Add "Add Alternate"

    [MenuItem("uRails/Create Rail")]
    static void CreateRail()
    {
        Object railManagerResource = Resources.Load(Assets.Resources.uRails.uRailManager);
        GameObject railManagerObject = (GameObject)PrefabUtility.InstantiatePrefab(railManagerResource);
        GameObject firstNode = CreateNewNode(null, "");

        firstNode.name = "uRailNode0";
        firstNode.transform.parent = railManagerObject.transform;
        firstNode.transform.position = railManagerObject.transform.position;
        firstNode.transform.rotation = railManagerObject.transform.rotation;
        firstNode.transform.localScale = railManagerObject.transform.localScale;

        railManagerObject.GetComponent<RailManager>().FirstNode = firstNode.GetComponent<RailNode>();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RailNode targetNode = Target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Successor 1"))
        {
            GameObject newNode = CreateNewNode(targetNode, "_main");
            Target.SuccessorNode1 = newNode.GetComponent<RailNode>();
        }

        if (GUILayout.Button("Add Successor 2"))
        {
            GameObject newNode = CreateNewNode(targetNode, "_alt");
            Target.SuccessorNode2 = newNode.GetComponent<RailNode>();
        }

        GUILayout.EndHorizontal();

        EditorUtility.SetDirty(Target);

    }

    private static GameObject CreateNewNode(RailNode targetNode, string label)
    {
        Object newNodeObject = Resources.Load(Assets.Resources.uRails.uRailNode);
        GameObject newNode = (GameObject)PrefabUtility.InstantiatePrefab(newNodeObject);

        if (targetNode != null)
        {
            newNode.transform.parent = targetNode.transform.parent;
            newNode.transform.position = targetNode.transform.position;
            newNode.transform.rotation = targetNode.transform.rotation;
            newNode.transform.localScale = targetNode.transform.localScale;
            newNode.name = GetNewNodeNameFromParent(targetNode.name) + label;
        }

        Selection.activeGameObject = newNode;

        return newNode;
    }

    private static string GetNewNodeNameFromParent(string parentName)
    {
        if (parentName.EndsWith("_main"))
        {
            parentName = parentName.Substring(0, parentName.Length - "_main".Length);
        }
        if (parentName.EndsWith("_alt"))
        {
            parentName = parentName.Substring(0, parentName.Length - "_alt".Length);
        }

        Regex regex = new Regex(@"(\d+)$",
                RegexOptions.Compiled |
                RegexOptions.CultureInvariant);

        Match match = regex.Match(parentName);

        if (match.Success)
        {
            int value;
            if (int.TryParse(match.Groups[1].Value, out value))
            {
                return parentName.Substring(0, parentName.Length - match.Groups[1].Value.Length) + (value + 1);
            }
            else
            {
                return parentName + "2";
            }
        }
        return parentName + "2";
    }
}