using UnityEngine;

public abstract class EventFilter : EventSender, IEventResponder, IOrderable
{
    [SerializeField]
    [HideInInspector]
    private int _executionTime;

    public int Ordinal
    {
        get { return _executionTime; }
        set { _executionTime = value; }
    }

    public virtual void Initialize()
    {
    }

    protected virtual void OnDestroy()
    {
    }

    public abstract void OnEvent(ExecutionContext context);

    public bool Enabled
    {
        get { return enabled; }
    }
}

