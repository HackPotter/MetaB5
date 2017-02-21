using UnityEngine;

[Trigger(Description = "Disables the given collider.", DisplayPath = "Physics")]
[AddComponentMenu("Metablast/Triggers/Actions/Disable Collider Action")]
public class DisableColliderAction : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private DisableColliderComponent _disableBehaviourComponent;
#pragma warning restore 0067, 0649

    protected override void Awake()
    {
        base.Awake();
        if (!_disableBehaviourComponent)
        {
            DebugFormatter.LogError(this, "DisableColliderComponent is null");
        }
    }

    public override void OnEvent(ExecutionContext context)
    {
        _disableBehaviourComponent.DisableComponent();
    }
}

