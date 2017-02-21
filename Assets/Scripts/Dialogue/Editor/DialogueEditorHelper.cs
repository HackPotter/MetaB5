using UnityEngine;

public static class DialogueEditorHelper
{
    public static void CreateDialogueSuccessor(GameObject parent, string speaker, string message)
    {
        GameObject successorObject = new GameObject();
        successorObject.transform.parent = parent.transform;
        successorObject.name = "Dialogue_" + speaker;

        DialogueNodeComponent nodeComponent = successorObject.AddComponent<DialogueNodeComponent>();
        nodeComponent.DialogueData = new DialogueNodeData();
        nodeComponent.DialogueData.Sender = speaker;
        nodeComponent.DialogueData.Message = message;
    }

    public static void CreateDialogueTransition(GameObject parent, string transitionText)
    {
        GameObject transitionObject = new GameObject();
        transitionObject.transform.parent = parent.transform;
        transitionObject.name = "DialogueTransition_" + transitionText;

        DialogueTransitionNodeComponent transitionComponent = transitionObject.AddComponent<DialogueTransitionNodeComponent>();
        transitionComponent.DialogueTransitionData = new DialogueTransitionNodeData();
        transitionComponent.DialogueTransitionData.TransitionText = transitionText;
    }

    public static void CreateJumpNode(GameObject parent, DialogueNodeComponent targetNode)
    {
        GameObject jumpObject = new GameObject();
        jumpObject.transform.parent = parent.transform;
        jumpObject.name = "DialogueJump_" + targetNode.name;

        DialogueJumpNode jumpNode = jumpObject.AddComponent<DialogueJumpNode>();
        jumpNode.JumpTarget = targetNode;
    }
}

