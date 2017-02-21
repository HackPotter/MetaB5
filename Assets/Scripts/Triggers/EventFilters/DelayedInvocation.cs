using System.Collections;
using UnityEngine;

[Trigger(Description = "Waits for the given amount of time before invoking all actions underneath.")]
[AddComponentMenu("Metablast/Triggers/Filters/Delayed Invocation")]
public class DelayedInvocation : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("Amount of time to wait before invoking the actions under this filter.")]
    private float _delaySeconds = 0.0f;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        StartCoroutine(DelayCoroutine(context));
    }

    private IEnumerator DelayCoroutine(ExecutionContext context)
    {
        yield return new WaitForSeconds(_delaySeconds);
        base.TriggerEvent(context);
    }
}