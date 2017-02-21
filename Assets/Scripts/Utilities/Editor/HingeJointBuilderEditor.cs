using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HingeJointBuilder))]
public class HingeJointBuilderEditor : Editor
{
    private HingeJointBuilder Target
    {
        get { return target as HingeJointBuilder; }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Target.Prefab)
        {
            EditorGUILayout.HelpBox("Add a prefab to build a HingeJoint chain", MessageType.Info);
        }
        else if (!Target.Prefab.GetComponent<Joint>())
        {
            EditorGUILayout.HelpBox("Prefab must have a Joint attached!", MessageType.Error);
        }
        else
        {
            if (GUILayout.Button("Create Chain"))
            {
                Target.BuildChain();
            }
        }
    }
}

