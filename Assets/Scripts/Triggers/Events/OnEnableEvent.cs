
[Trigger(Description = "Invoked each time this trigger is enabled.", UserFriendlySegment = "When this trigger is enabled, ",DisplayPath="Initialization")]
public class OnEnableEvent : EventSender
{
    void OnEnable()
    {
        TriggerEvent();
    }
}
