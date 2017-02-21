using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CallUnityFunctionEvent : UnityEvent { }

[Trigger(Description = "Calls a parameterless function on a component.", DisplayPath="GameObject")]
public class CallUnityFunction : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private CallUnityFunctionEvent _unityFunction;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        _unityFunction.Invoke();
    }
}
