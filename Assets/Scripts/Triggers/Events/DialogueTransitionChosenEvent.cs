using UnityEngine;

[Trigger(Description= "Invoked when the player has made a choice after having been given dialogue with multiple branches.", DisplayPath = "Dialogue")]
[AddComponentMenu("Metablast/Triggers/Events/Dialogue/Dialogue Transition Chosen")]
public class DialogueTransitionChosenEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The dialogue transition node for which this event will be invoked when the player has made a decision.")]
    private DialogueTransitionNodeComponent _transitionNode;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        base.TriggerRoot.DialogueController.OnDialogueBranchTaken += Instance_OnDialogueBranchTaken;
    }

    void OnDestroy()
    {
        base.TriggerRoot.DialogueController.OnDialogueBranchTaken -= Instance_OnDialogueBranchTaken;
    }

    void Instance_OnDialogueBranchTaken(DialogueNodeComponent currentNode, DialogueTransitionNodeComponent node)
    {
        if (enabled && gameObject.activeInHierarchy && node == _transitionNode)
        {
            TriggerEvent();
        }
    }
}

