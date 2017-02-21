using UnityEngine;

[Trigger(Description="Invoked exactly once before the first frame the Trigger is active.",
         UserFriendlySegment="When the scene is loaded, ",
         DisplayPath = "Initialization")]
[AddComponentMenu("Metablast/Triggers/Events/OnAwake")]
public class OnAwakeEvent : EventSender
{
    protected override void OnAwake()
    {
        TriggerEvent();
    }
}