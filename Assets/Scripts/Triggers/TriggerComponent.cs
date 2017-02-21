using UnityEngine;

public abstract class TriggerComponent : MonoBehaviour
{
    private bool _initialized = false;
    private TriggerRoot _root;

    protected TriggerRoot TriggerRoot
    {
        get
        {
            Initialize();
            return _root;
        }
    }
    private void Initialize()
    {
        if (_initialized)
            return;

        _initialized = true;
        _root = GetComponentInParent<TriggerRoot>();
    }

    protected virtual void Awake() { }
}