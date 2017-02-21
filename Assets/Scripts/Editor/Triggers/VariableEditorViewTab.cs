using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class VariableEditorViewTab
{
    private EventEditorContext _context;

    private Vector2 _variableScrollView;

    private string _createVariableIdentifier = "";
    private int _createVariableTypeIndex = 0;

    private HashSet<Variable> _expandedViews = new HashSet<Variable>();

    public EventEditorContext Context
    {
        get
        {
            return _context;
        }
        set
        {
            _context = value;
        }
    }

    public void Draw()
    {
        GlobalSymbolTable table = _context.GlobalSymbolTable;

        _variableScrollView = GUILayout.BeginScrollView(_variableScrollView, GUI.skin.box, GUILayout.ExpandHeight(false));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_expandedViews.Count > 0 ? "-" : "+", GUILayout.Width(20), GUILayout.Height(20)))
        {
            if (_expandedViews.Count > 0)
            {
                _expandedViews.Clear();
            }
            else
            {
                foreach (Variable variable in table.Variables.Values)
                {
                    _expandedViews.Add(variable);
                }
            }
        }
        GUI.skin.label.fontSize = 18;
        GUILayout.Label("Variables", GUILayout.ExpandHeight(false));
        GUI.skin.label.fontSize = 11;
        GUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginVertical();

        foreach (var item in table.Variables.Where(kvp => kvp.Value == null).ToList())
        {
            table.Variables.Remove(item.Key);
        }

        List<KeyValuePair<string, Variable>> toDelete = new List<KeyValuePair<string,Variable>>();
        foreach (var kvp in table.Variables)
        {
            string identifier = kvp.Key;
            Variable variable = kvp.Value;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_expandedViews.Contains(variable) ? "-" : "+", GUILayout.Width(20), GUILayout.Height(15)))
            {
                if (_expandedViews.Contains(variable))
                {
                    _expandedViews.Remove(variable);
                }
                else
                {
                    _expandedViews.Add(variable);
                }
            }
            GUILayout.Label(identifier + " (" + variable.GetType().Name + ")", GUILayout.ExpandWidth(false));

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Delete", GUILayout.Width(50), GUILayout.Height(15)))
            {
                toDelete.Add(kvp);
            }
            GUILayout.EndHorizontal();

            if (_expandedViews.Contains(variable))
            {
                SerializedObject serializedVariable = new SerializedObject(variable);
                SerializedProperty property = serializedVariable.GetIterator();
                property.NextVisible(true);
                while (property.NextVisible(false))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    EditorGUILayout.PropertyField(property, true, GUILayout.ExpandWidth(false));
                    serializedVariable.ApplyModifiedProperties();
                    GUILayout.EndHorizontal();
                }
            }
        }
        foreach (var variable in toDelete)
        {
            table.Variables.Remove(variable.Key);
        }
        toDelete.ForEach((v) => ScriptableObject.DestroyImmediate(v.Value));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        GUILayout.EndScrollView();

        // Get all Variable types.
        Dictionary<string, Type> typesByName = Assembly.GetAssembly(typeof(Variable)).GetTypes().
                                       Where((t) => typeof(Variable).IsAssignableFrom(t) && !t.IsAbstract && t != typeof(DynamicVariable)).
                                       ToDictionary((t) => t.Name);

        EditorGUILayout.BeginHorizontal();
        bool validIdentifier = !string.IsNullOrEmpty(_createVariableIdentifier);
        bool availableIdentifier = !table.Variables.ContainsKey(_createVariableIdentifier);
        if (GUILayout.Button("Create New Variable", GUILayout.ExpandWidth(false)))
        {
            if (validIdentifier && availableIdentifier)
            {
                Variable newVariable = (Variable)ScriptableObject.CreateInstance(typesByName.Keys.ToArray()[_createVariableTypeIndex]);
                table.AddVariable(_createVariableIdentifier, newVariable);

                _createVariableIdentifier = "";
                _createVariableTypeIndex = 0;
            }
        }
        if (!availableIdentifier)
        {
            GUILayout.Label("Variable identifier already in use!");
        }
        else if (!validIdentifier)
        {
            GUILayout.Label("Invalid variable identifier!");
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Identifier: ");
        _createVariableIdentifier = GUILayout.TextField(_createVariableIdentifier);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Type: ");
        _createVariableTypeIndex = EditorGUILayout.Popup(_createVariableTypeIndex, typesByName.Keys.ToArray());
        GUILayout.EndHorizontal();
    }
}

