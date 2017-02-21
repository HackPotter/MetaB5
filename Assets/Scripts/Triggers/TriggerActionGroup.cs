using UnityEngine;
using System.Collections.Generic;

public class TriggerActionGroup : TriggerComponent
{
    public TriggerActionGroupDescriptor Descriptor
    {
        get { return _triggerActionGroupDescriptor; }
    }

    public static GameObject CreateActionGroup(TriggerActionGroupDescriptor descriptor)
    {
        GameObject gameObject = new GameObject(descriptor.ActionGroupName);
        gameObject.AddComponent<TriggerActionGroup>()._triggerActionGroupDescriptor = descriptor;
        return gameObject;
    }

    [SerializeField]
    private TriggerActionGroupDescriptor _triggerActionGroupDescriptor;
    
    private List<EventResponder> _responders;

    protected override void Awake()
    {
        base.Awake();
        _responders = new List<EventResponder>();

        foreach (Transform child in transform)
        {
            _responders.AddRange(child.GetComponents<EventResponder>());
        }

        _responders.Sort((r1, r2) => { return r1.Ordinal - r2.Ordinal; });
    }

    public void Trigger(ExecutionContext context)
    {
        foreach (EventResponder responder in _responders)
        {
            responder.OnEvent(context);
        }
    }
}

