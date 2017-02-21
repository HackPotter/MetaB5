using UnityEngine;

[Trigger(Description = "Invoked when the player has finished reading a page of dialogue and has chosen to continue to the next.", DisplayPath = "Dialogue")]
[AddComponentMenu("Metablast/Triggers/Events/Dialogue/Dialogue Node Completed Event")]
public class DialogueNodeCompletedEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The dialogue node for which this event will be invoked when completed.")]
    private DialogueNodeComponent _dialogueNode;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        base.TriggerRoot.DialogueController.OnDialogueNodeCompleted += Instance_OnDialogueNodeCompleted;
    }

    void OnDestroy()
    {
		if (!base.TriggerRoot)
		{
			Debug.Log("Trying to unsubscribe from DialogueController, but TriggerRoot is null");
			return;
		}
		if (!base.TriggerRoot.DialogueController)
		{
			Debug.Log("Trying to unsubscribe from DialogueController, but TriggerRoot.DialogueController is null");
			return;
		}
        base.TriggerRoot.DialogueController.OnDialogueNodeCompleted -= Instance_OnDialogueNodeCompleted;
    }

    void Instance_OnDialogueNodeCompleted(DialogueNodeComponent node)
    {
        if (node == _dialogueNode)
        {
            TriggerEvent();
        }
    }
}
