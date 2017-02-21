using UnityEngine;

[Trigger(Description="Invoked exactly once before the first frame the Trigger is active.",
         UserFriendlySegment="When the scene is loaded, ",
         DisplayPath = "Initialization")]
[AddComponentMenu("Metablast/Triggers/Events/On Start")]
public class OnStartEvent : EventSender
{
    protected override void OnStart()
    {
        TriggerEvent();
    }
}