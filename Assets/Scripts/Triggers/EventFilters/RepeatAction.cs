using System.Collections;
using UnityEngine;

[Trigger(Description = "Repeats all actions under this filter the specified number of times, with the specified delay. If the delay is 0, the actions are repeated within the same frame.")]
[AddComponentMenu("Metablast/Triggers/Filters/Repeat Action")]
public class RepeatAction : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The number of times to invoke the actions.")]
    private int _numberOfTimesToRepeat;

    [SerializeField]
    [Infobox("The amount of time allowed to pass between invocations.")]
    private float _delayBetweenActionsSeconds;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_delayBetweenActionsSeconds == 0)
        {
            for (int i = 0; i < _numberOfTimesToRepeat; i++)
            {
                TriggerEvent(context);
            }
        }
        else
        {
            StartCoroutine(RepeatCoroutine(context));
        }
    }

    private IEnumerator RepeatCoroutine(ExecutionContext context)
    {
        for (int i = 0; i < _numberOfTimesToRepeat; i++)
        {
            TriggerEvent(context);
            yield return new WaitForSeconds(_delayBetweenActionsSeconds);
        }
    }
}

