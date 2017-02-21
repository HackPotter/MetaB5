using UnityEngine;

[Trigger(Description = "Invoked the first frame the user has begun to hover the mouse over the given MouseCollider.", DisplayPath = "Input")]
[AddComponentMenu("Metablast/Triggers/Events/Input/Mouse Entered Interactable")]
public class MouseEnteredInteractable : EventSender
{

#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The interactable for which this event will be invoked when the mouse has hovered over it.")]
    private MouseCollider _interactable;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        if (_interactable == null)
        {
            DebugFormatter.Log(this, "Interactable object is null.");
            return;
        }

        _interactable.MouseEntered += new MouseEnterInteractable(Interactable_MouseEntered);
    }

    void Interactable_MouseEntered(MouseCollider target)
    {
        TriggerEvent();
    }
}

