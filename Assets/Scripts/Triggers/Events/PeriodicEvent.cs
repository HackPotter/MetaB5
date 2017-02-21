using System.Collections;
using UnityEngine;

[Trigger(Description = "Invoked periodically with the given period in seconds.")]
[AddComponentMenu("Metablast/Triggers/Events/Periodic Event")]
public class PeriodicEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The amount of time between invocations of this event.")]
    private float _periodSeconds = 1.0f;
#pragma warning restore 0067, 0649

    void OnEnable()
    {
        StartCoroutine(PeriodicCoroutine());
    }

    private IEnumerator PeriodicCoroutine()
    {
        while (true)
        {
            TriggerEvent();
            yield return new WaitForSeconds(_periodSeconds);
        }
    }
}

