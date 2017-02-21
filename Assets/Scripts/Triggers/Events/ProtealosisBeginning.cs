
[Trigger(DisplayPath = "Biology")]
public class ProtealosisBeginning : EventSender
{
    protected override void OnStart()
    {
        ProteasomeAgent.ProtealosisBeginning += ProteasomeAgent_Protealyzed;
    }

    void OnDestroy()
    {
        ProteasomeAgent.ProtealosisBeginning -= ProteasomeAgent_Protealyzed;
    }

    void ProteasomeAgent_Protealyzed()
    {
        TriggerEvent();
    }
}

