using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActionGroupEditorNode : TriggerEditorNode
{
    private TriggerActionGroup _actionGroup;

    public TriggerActionGroup TriggerActionGroup
    {
        get { return _actionGroup; }
    }

    public ActionGroupEditorNode(TriggerActionGroup triggerActionGroup, EventEditorContext context) : base(context)
    {
        _actionGroup = triggerActionGroup;
    }

    public override void DrawGUI()
    {
        //if (Expanded)
        //{
            //EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            //GUILayout.Space(25);
            
            //EditorGUILayout.BeginVertical();

            //GUILayout.BeginVertical(GUI.skin.box);

            //EditorGUILayout.BeginHorizontal();
            //GUILayout.FlexibleSpace();
            //GUILayout.Box("", GUILayout.Width(300), GUILayout.Height(1));
            //EditorGUILayout.EndHorizontal();

            //TriggerGUILayout.DrawAddFilterSelector(TriggerComponent.gameObject);
            //TriggerGUILayout.DrawAddActionSelector(TriggerComponent.gameObject);

            //EditorGUILayout.Separator();

            //GUILayout.EndVertical();
            //EditorGUILayout.EndVertical();
            //EditorGUILayout.EndHorizontal();
        //}
    }

    public override TriggerComponent TriggerComponent
    {
        get { return TriggerActionGroup; }
    }

    public override Dictionary<string, Variable> GetOutputVariables()
    {
        return new Dictionary<string, Variable>();
    }

    public override Dictionary<string, Variable> GetScopeVariables()
    {
        Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
        foreach (var kvp in Parent.GetScopeVariables())
        {
            variables.Add(kvp.Key, kvp.Value);
        }
        /*
        foreach (var kvp in _outputVariables)
        {
            variables.Add(kvp.Key, kvp.Value);
        }*/

        return variables;
    }

    protected override void OnNodeDeleted()
    {
    }
}

