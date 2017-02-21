using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueNodeComponent))]
public class DialogueNodeComponentEditor : Editor
{
    private string _transitionNodeText = "";

    private string _speaker = "";
    private string _message = "";

    private DialogueNodeComponent _jumpTarget;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueNodeComponent nodeTarget = (DialogueNodeComponent)target;

        if (nodeTarget.DialogueData == null)
        {
            nodeTarget.DialogueData = new DialogueNodeData();
        }

        GUILayout.Label("Sender");
        nodeTarget.DialogueData.Sender = GUILayout.TextField(nodeTarget.DialogueData.Sender);
        GUILayout.Label("Message");
        nodeTarget.DialogueData.Message = GUILayout.TextArea(nodeTarget.DialogueData.Message);


        nodeTarget.PausesGame = GUILayout.Toggle(nodeTarget.PausesGame, "Pause Game");

        GUILayout.Space(25);

        if (GUILayout.Button("Add Transition Node", GUILayout.ExpandWidth(false)))
        {
            CreateTransitionNode();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Transition Text:", GUILayout.ExpandWidth(false));
        _transitionNodeText = GUILayout.TextField(_transitionNodeText);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
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

    private void CreateTransitionNode()
    {
        DialogueEditorHelper.CreateDialogueTransition((target as DialogueNodeComponent).gameObject, _transitionNodeText);
        _transitionNodeText = "";
    }

    private void CreateSuccessorNode()
    {
        DialogueEditorHelper.CreateDialogueSuccessor((target as DialogueNodeComponent).gameObject, _speaker, _message);
        _speaker = "";
        _message = "";
    }

    private void CreateJumpNode()
    {
        DialogueEditorHelper.CreateJumpNode((target as DialogueNodeComponent).gameObject, _jumpTarget);
        _jumpTarget = null;
    }
}
