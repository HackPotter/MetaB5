using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueTransitionNodeComponent))]
public class DialogueTransitionNodeComponentEditor : Editor
{
    private string _speaker = "";
    private string _message = "";

    private DialogueNodeComponent _jumpTarget;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(25);

        if (GUILayout.Button("Add Dialogue Node", GUILayout.ExpandWidth(false)))
        {
            CreateSuccessorNode();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Speaker", GUILayout.ExpandWidth(false));
        _speaker = GUILayout.TextField(_speaker);
        GUILayout.EndHorizontal();
        GUILayout.Label("Message:", GUILayout.ExpandWidth(false));
        _message = GUILayout.TextArea(_message);

        GUILayout.Space(20);
        if (GUILayout.Button("Add Jump Node", GUILayout.ExpandWidth(false)))
        {
            CreateJumpNode();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Jump Target", GUILayout.ExpandWidth(false));
        _jumpTarget = EditorGUILayout.ObjectField(_jumpTarget, typeof(DialogueNodeComponent), true) as DialogueNodeComponent;
        GUILayout.EndHorizontal();
    }

    private void CreateSuccessorNode()
    {
        DialogueEditorHelper.CreateDialogueSuccessor((target as DialogueTransitionNodeComponent).gameObject, _speaker, _message);
        _speaker = "";
        _message = "";
    }

    private void CreateJumpNode()
    {
        DialogueEditorHelper.CreateJumpNode((target as DialogueTransitionNodeComponent).gameObject, _jumpTarget);
        _jumpTarget = null;
    }
}
