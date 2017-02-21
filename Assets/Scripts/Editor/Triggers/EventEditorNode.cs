using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventEditorNode : TriggerEditorNode
{
    private GameObject _gameObject;
    private EventSender _sender;
    private SerializedObject _serializedEvent;

    private Dictionary<string, Variable> _outputVariables = new Dictionary<string, Variable>();

    public EventSender Sender
    {
        get { return _sender; }
        set
        {
            _sender = value;

            foreach (Variable v in _outputVariables.Values)
            {
                ScriptableObject.DestroyImmediate(v);
            }

            _outputVariables.Clear();

            if (!_sender)
            {
                return;
            }
            _serializedEvent = new SerializedObject(value);
            foreach (OutputParameterDeclaration outputDeclaration in Sender.GetOutputParameterDeclarations())
            {
                DynamicVariable v = ScriptableObject.CreateInstance<DynamicVariable>();
                v.SetDynamicType(outputDeclaration.Type);
                _outputVariables.Add(outputDeclaration.Name, v);
            }
        }
    }

    public EventEditorNode(GameObject eventGameObject, EventEditorContext context)
        : base(context)
    {
        _gameObject = eventGameObject;
        Sender = _gameObject.GetComponent<EventSender>();
    }

    public override TriggerComponent TriggerComponent
    {
        get { return Sender; }
    }

    public override void DrawGUI()
    {
        EventSender returnedSender;
        Expanded = TriggerGUILayout.DrawCustomEventInspectorBar(Expanded, _gameObject, out returnedSender);
        if (Sender != returnedSender)
        {
            Sender = returnedSender;
        }

        if (Sender == null)
        {
            return;
        }

        if (Expanded)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Space(25);
            EditorGUILayout.BeginVertical();

            GUILayout.BeginVertical(GUI.skin.box);

            TriggerAttribute triggerAttribute = TriggerGUILayout.GetTriggerAttribute(Sender.GetType());
            if (triggerAttribute != null)
            {
                GUILayout.Label(triggerAttribute.Description, EditorStyles.wordWrappedLabel, GUILayout.ExpandWidth(false));
            }

            if (_serializedEvent.GetIterator().CountRemaining() > 1)
            {
                EditorGUILayoutExt.BeginLabelStyle(11, FontStyle.Bold, null, null);
                GUILayout.Label("Input Parameters");
                EditorGUILayoutExt.EndLabelStyle();
            }
            TriggerGUILayout.DrawSerializedObject(_serializedEvent, Sender.GetType(), Context.GlobalSymbolTable.Variables);

            
            if (Sender.GetOutputParameterDeclarations().Count > 0)
            {
                EditorGUILayout.Separator();
                EditorGUILayoutExt.BeginLabelStyle(11, FontStyle.Bold, null, null);
                GUILayout.Label("Output Parameters");
                EditorGUILayoutExt.EndLabelStyle();
                
                foreach (var v in Sender.GetOutputParameterDeclarations())
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(15);
                    GUILayout.Label(v.Name + " (" + v.Type.Name + ")", GUILayout.ExpandWidth(false));
                    if (!string.IsNullOrEmpty(v.Description))
                    {
                        GUILayout.Label(v.Description);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box("", GUILayout.Width(300), GUILayout.Height(1));
            EditorGUILayout.EndHorizontal();

            TriggerGUILayout.DrawAddFilterSelector(Sender.gameObject, Context.Refresh);
            TriggerGUILayout.DrawAddActionSelector(Sender.gameObject, Context.Refresh);

            EditorGUILayout.Separator();
            GUILayout.EndVertical();

            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

    public override Dictionary<string, Variable> GetScopeVariables()
    {
        Dictionary<string, Variable> variables = new Dictionary<string, Variable>(Context.GlobalSymbolTable.Variables);

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