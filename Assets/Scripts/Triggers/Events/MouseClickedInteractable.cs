using UnityEngine;
using UnityEngine.EventSystems;

[Trigger(Description = "Invoked when the given MouseCollider has been clicked by the user.", DisplayPath = "Input")]
[AddComponentMenu("Metablast/Triggers/Events/Input/Mouse Clicked Interactable")]
public class MouseClickedInteractable : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The mouse collider for which this event will be invoked when clicked.")]
    private MouseCollider _interactableObject;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        if (_interactableObject == null)
        {
            DebugFormatter.Log(this, "Interactable object is null");
            return;
        }

        _interactableObject.MouseClicked += new MouseClickInteractable(_interactableObject_MouseClicked);
    }

    void _interactableObject_MouseClicked(MouseCollider target)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            TriggerEvent();
        }
    }
}

