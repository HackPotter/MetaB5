using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class FilterEditorNode : TriggerEditorNode
{
    private EventFilter _filter;
    private SerializedObject _serializedFilter;

    public EventFilter Filter
    {
        get { return _filter; }
        private set
        {
            _filter = value;
            _serializedFilter = new SerializedObject(_filter);

            _outputVariables = new Dictionary<string, Variable>();
            foreach (OutputParameterDeclaration outputDeclaration in Filter.GetOutputParameterDeclarations())
            {
                DynamicVariable v = ScriptableObject.CreateInstance<DynamicVariable>();
                v.SetDynamicType(outputDeclaration.Type);
                _outputVariables.Add(outputDeclaration.Name, v);
            }

            if (value.gameObject.name != value.GetType().Name)
            {
                value.gameObject.name = value.GetType().Name;
            }
        }
    }

    private Dictionary<string, Variable> _outputVariables;

    public FilterEditorNode(EventFilter filter, EventEditorContext context)
        : base(context)
    {
        Filter = filter;
    }

    public override TriggerComponent TriggerComponent
    {
        get { return Filter; }
    }

    public override void DrawGUI()
    {
        EventFilter returnedFilter;
        Expanded = TriggerGUILayout.DrawCustomFilterInspectorBar(Expanded, Filter, out returnedFilter);

        if (!returnedFilter)
        {
            foreach (Variable var in _outputVariables.Values)
            {
                ScriptableObject.DestroyImmediate(var);
            }
            Delete();
            return;
        }
        Filter = returnedFilter;

        if (Expanded)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Space(25);
            EditorGUILayout.BeginVertical();

            GUILayout.BeginVertical(GUI.skin.box);

            TriggerAttribute triggerAttribute = TriggerGUILayout.GetTriggerAttribute(Filter.GetType());
            if (triggerAttribute != null)
            {
                GUILayout.Label(triggerAttribute.Description, EditorStyles.wordWrappedLabel, GUILayout.ExpandWidth(false));
            }

            EditorGUILayoutExt.BeginLabelStyle(11, FontStyle.Bold, null, null);
            GUILayout.Label("Input Parameters");
            EditorGUILayoutExt.EndLabelStyle();

            TriggerGUILayout.DrawSerializedObject(_serializedFilter, Filter.GetType(), Parent.GetScopeVariables());
            
            EditorGUILayout.Separator();

            EditorGUILayoutExt.BeginLabelStyle(11, FontStyle.Bold, null, null);
            GUILayout.Label("Output Parameters");
            EditorGUILayoutExt.EndLabelStyle();

            foreach (var v in Filter.GetOutputParameterDeclarations())
            {
                GUILayout.Label(v.Name, GUILayout.ExpandWidth(false));
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box("", GUILayout.Width(300), GUILayout.Height(1));
            EditorGUILayout.EndHorizontal();

            TriggerGUILayout.DrawAddFilterSelector(Filter.gameObject, Context.Refresh);
            TriggerGUILayout.DrawAddActionSelector(Filter.gameObject, Context.Refresh);

            EditorGUILayout.Separator();

            GUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

    public override Dictionary<string, Variable> GetScopeVariables()
    {
        Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
        foreach (var kvp in Parent.GetScopeVariables())
        {
            variables.Add(kvp.Key, kvp.Value);
        }
        foreach (var kvp in _outputVariables)
        {
            variables.Add(kvp.Key, kvp.Value);
        }

        return variables;
    }

    public override Dictionary<string, Variable> GetOutputVariables()
    {
        return _outputVariables;
    }

    protected override void OnNodeDeleted()
    {
        foreach (Variable v in _outputVariables.Values)
        {
            ScriptableObject.DestroyImmediate(v);
        }
        _outputVariables.Clear();
    }
}