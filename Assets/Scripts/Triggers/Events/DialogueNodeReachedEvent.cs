using UnityEngine;

[Trigger(Description="Invoked when the player has reached a new page of dialogue.", DisplayPath = "Dialogue")]
[AddComponentMenu("Metablast/Triggers/Events/Dialogue/Dialogue Node Reached")]
public class DialogueNodeReachedEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The dialogue node for which this event will be invoked when reached.")]
    private DialogueNodeComponent _dialogueNode;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        base.TriggerRoot.DialogueController.OnDialogueNodeReached += Instance_OnDialogueNodeReached;
    }

    void OnDestroy()
    {
        base.TriggerRoot.DialogueController.OnDialogueNodeReached -= Instance_OnDialogueNodeReached;
    }

    void Instance_OnDialogueNodeReached(DialogueNodeComponent node)
    {
        if (enabled && gameObject.activeInHierarchy && node == _dialogueNode)
        {
            TriggerEvent();
        }
    }
}
