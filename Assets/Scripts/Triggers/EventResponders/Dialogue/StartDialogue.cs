using UnityEngine;

[Trigger(Description = "Starts dialogue with the given dialogue node.", DisplayPath = "Dialogue")]
[AddComponentMenu("Metablast/Triggers/Actions/Dialogue/Start Dialogue")]
public class StartDialogue : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private DialogueNodeComponent _dialogueNode;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        base.TriggerRoot.DialogueController.StartDialogue(_dialogueNode);
    }
}

