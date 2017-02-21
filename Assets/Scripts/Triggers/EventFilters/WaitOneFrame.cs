using System.Collections;
using UnityEngine;

[AddComponentMenu("Metablast/Triggers/Filters/Wait One Frame")]
[Trigger(Description = "Waits one frame before invoking the actions under this filter.")]
public class WaitOneFrame : EventFilter
{
    public override void OnEvent(ExecutionContext context)
    {
        StartCoroutine(WaitOneFrameCoroutine(context));
    }

    private IEnumerator WaitOneFrameCoroutine(ExecutionContext context)
    {
        yield return null;
        TriggerEvent(context);
    }
}

