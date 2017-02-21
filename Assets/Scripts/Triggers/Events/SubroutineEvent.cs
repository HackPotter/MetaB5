using UnityEngine;

[Trigger(Description ="Invoked when an ExecuteSubroutine action is executed.")]
[AddComponentMenu("Metablast/Triggers/Events/Subroutine Event")]
public class SubroutineEvent : EventSender
{
    public void InvokeSubroutine()
    {
        TriggerEvent();
    }
}
