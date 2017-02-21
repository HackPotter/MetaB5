using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ActionEditorNode : TriggerEditorNode
{
    private SerializedObject _serializedAction;
    private EventResponder _action;

    public EventResponder Action
    {
        get
        {
            return _action;
        }
        private set
        {
            _action = value;
            _serializedAction = new SerializedObject(_action);

            if (value.gameObject.name != value.GetType().Name)
            {
                value.gameObject.name = value.GetType().Name;
            }
        }
    }

    public ActionEditorNode(EventResponder responder, EventEditorContext context)
        : base(context)
    {
        Action = responder;
    }

    public override TriggerComponent TriggerComponent
    {
        get { return Action; }
    }

    public override void DrawGUI()
    {
        EventResponder returnedResponder;
        Expanded = TriggerGUILayout.DrawCustomActionInspectorBar(Expanded, Action, out returnedResponder);

        if (!returnedResponder)
        {
            Delete();
            return;
        }
        if (returnedResponder != Action)
        {
            Action = returnedResponder;
        }

        if (Expanded)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Space(25);
            EditorGUILayout.BeginVertical();

            GUILayout.BeginVertical(GUI.skin.box);

            TriggerAttribute triggerAttribute = TriggerGUILayout.GetTriggerAttribute(Action.GetType());
            if (triggerAttribute != null)
            {
                GUILayout.Label(triggerAttribute.Description, EditorStyles.wordWrappedLabel, GUILayout.ExpandWidth(false));
            }

            EditorGUILayoutExt.BeginLabelStyle(11, FontStyle.Bold, null, null);
            GUILayout.Label("Input Parameters");
            EditorGUILayoutExt.EndLabelStyle();
            
            TriggerGUILayout.DrawSerializedObject(_serializedAction, Action.GetType(), GetScopeVariables());
            EditorGUILayout.Separator();

            GUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

    public override Dictionary<string, Variable> GetScopeVariables()
    {
        return Parent.GetScopeVariables();
    }

    public override Dictionary<string, Variable> GetOutputVariables()
    {
        return new Dictionary<string, Variable>();
    }

    protected override void OnNodeDeleted()
    {
    }
}