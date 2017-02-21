using System.Collections.Generic;
using UnityEngine;

public abstract class EventResponder : TriggerComponent, IEventResponder
{
    [SerializeField]
    [HideInInspector]
    private int _executionTime;

    private Dictionary<string, TriggerActionGroup> _triggerActionGroups = new Dictionary<string, TriggerActionGroup>();

    public int Ordinal
    {
        get { return _executionTime; }
        set { _executionTime = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        foreach (Transform child in transform)
        {
            TriggerActionGroup actionGroup = child.GetComponent<TriggerActionGroup>();
            if (actionGroup)
            {
                // TODO make sure they all exist.
                _triggerActionGroups.Add(actionGroup.Descriptor.ActionGroupName, actionGroup);
            }
        }
    }

    protected virtual void Start()
    {
    }

    protected virtual void OnDestroy()
    {
    }

    public bool Enabled
    {
        get { return enabled && gameObject.activeInHierarchy; }
    }

    public virtual List<TriggerActionGroupDescriptor> GetTriggerActionGroups()
    {
        return new List<TriggerActionGroupDescriptor>();
    }

    public abstract void OnEvent(ExecutionContext context);

    protected void TriggerActionGroup(TriggerActionGroupDescriptor triggerActionGroup, ExecutionContext context)
    {
        _triggerActionGroups[triggerActionGroup.ActionGroupName].Trigger(context);
    }
}

