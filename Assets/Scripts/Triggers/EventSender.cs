using System;
using System.Collections.Generic;
using UnityEngine;

public class OutputParameterDeclaration
{
    public string Name;
    public string Description;
    public Type Type;
}

public abstract class EventSender : TriggerComponent
{
    private List<IEventResponder> _responders = new List<IEventResponder>();

    protected override void Awake()
    {
        base.Awake();
        foreach (Transform child in transform)
        {
            EventFilter[] filters = child.GetComponents<EventFilter>();
            EventResponder[] responders = child.GetComponents<EventResponder>();
            if (filters.Length + responders.Length > 1)
            {
                DebugFormatter.LogError(this, "GameObject {0} has more than one filter or action.", child.name);
                return;
            }

            foreach (EventFilter filter in filters)
            {
                _responders.Add(filter);
            }

            foreach (EventResponder responder in responders)
            {
                _responders.Add(responder);
            }
        }

        _responders.Sort((r1, r2) => { return r1.Ordinal - r2.Ordinal; });

        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnAwake() { }

    protected virtual void OnStart() { }

    protected virtual void PopulateContext(ExecutionContext context)
    {
    }

    public virtual List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return new List<OutputParameterDeclaration>();
    }

    protected virtual void TriggerEvent(ExecutionContext incomingContext)
    {
        if (!enabled || !gameObject.activeInHierarchy)
        {
            return;
        }

        foreach (var responder in _responders)
        {
            if (!responder.Enabled)
            {
                continue;
            }
            ExecutionContext context = new ExecutionContext(incomingContext);
            PopulateContext(context);
            responder.OnEvent(context);
        }
    }

    protected virtual void TriggerEvent()
    {
        if (!enabled || !gameObject.activeInHierarchy)
        {
            return;
        }

        ExecutionContext context = new ExecutionContext(base.TriggerRoot.GlobalSymbolTable);
        TriggerEvent(context);
    }
}

