
[Trigger(DisplayPath = "Biology")]
public class ProtealosisOccurredEvent : EventSender
{
    protected override void OnStart()
    {
        ProteasomeAgent.Protealyzed += ProteasomeAgent_Protealyzed;
    }

    void OnDestroy()
    {
        ProteasomeAgent.Protealyzed -= ProteasomeAgent_Protealyzed;
    }

    void ProteasomeAgent_Protealyzed()
    {
        TriggerEvent();
    }
}

