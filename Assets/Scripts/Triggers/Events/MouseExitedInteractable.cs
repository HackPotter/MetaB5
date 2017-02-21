using UnityEngine;

[Trigger(Description = "Invoked the first frame the user has stoped hovering the mouse over the given MouseCollider.", DisplayPath = "Input")]
[AddComponentMenu("Metablast/Triggers/Events/Input/Mouse Exited Interactable")]
public class MouseExitedInteractable : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The mouse collider for which this event will be invoked.")]
    private MouseCollider _interactableObject;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        if (_interactableObject == null)
        {
            DebugFormatter.Log(this, "Interactable object is null");
            return;
        }

        _interactableObject.MouseLeft += new MouseLeaveInteractable(_interactableObject_MouseLeft);
    }

    void _interactableObject_MouseLeft(MouseCollider target)
    {
        TriggerEvent();
    }
}

