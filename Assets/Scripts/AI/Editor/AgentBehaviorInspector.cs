#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AgentBehavior))]
public class AgentBehaviorInspector : Editor
{
    private AgentBehavior Target
    {
        get { return base.target as AgentBehavior; }
    }

    private static Type[] _steeringTypes;
    private static GUIContent[] _steeringTypesContent;

    void OnEnable()
    {
        if (_steeringTypes == null)
        {
            _steeringTypes = Assembly.GetAssembly(typeof(Steering)).GetTypes().Where((t) => typeof(Steering).IsAssignableFrom(t) && !t.IsAbstract).ToArray();

            _steeringTypesContent = new GUIContent[_steeringTypes.Length];

            int index = 0;
            foreach (Type type in _steeringTypes)
            {
                _steeringTypesContent[index++] = new GUIContent(type.Name);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        int selectedIndex = EditorGUILayout.Popup(new GUIContent("Add Steering Component"), -1, _steeringTypesContent);
        if (selectedIndex >= 0 && selectedIndex < _steeringTypes.Length)
        {
            Type selected = _steeringTypes[selectedIndex];
            Target.gameObject.AddComponent(selected);
        }

        List<Steering> toDelete = new List<Steering>();
        /*
        foreach (Steering steering in Target.SteeringComponents)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(steering.GetType().Name);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("x"))
            {
                toDelete.Add(steering);
            }
            GUILayout.EndHorizontal();
            SerializedObject serializedSteering = new SerializedObject(steering);

            SerializedProperty prop = serializedSteering.GetIterator();
            prop.Next(true);
            prop.NextVisible(true);
            while (prop.NextVisible(true))
            {
                EditorGUILayout.PropertyField(prop);
            }
            serializedSteering.ApplyModifiedProperties();
            serializedSteering.Update();

            GUILayout.EndVertical();

            //Type steeringType = steering.GetType();
            //GUILayout.Label(steering.GetType().Name);
        }

        foreach (Steering deleted in toDelete)
        {
            Target.SteeringComponents.Remove(deleted);
        }*/
    }
}