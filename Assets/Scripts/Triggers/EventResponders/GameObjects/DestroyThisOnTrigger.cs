using UnityEngine;

[Trigger(Description="DEPRECATED. Destroying triggers is not recommended. Use DisableGameObject instead.", DisplayPath = "Deprecated")]
[AddComponentMenu("Metablast/Triggers/Actions/GameObjects/Destroy This On Trigger")]
public class DestroyThisOnTrigger : EventResponder
{
    public override void OnEvent(ExecutionContext context)
    {
        Destroy(this.gameObject);
    }
}
