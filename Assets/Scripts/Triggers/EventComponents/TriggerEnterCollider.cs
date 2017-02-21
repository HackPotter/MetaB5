using System.Collections.Generic;
using UnityEngine;

public delegate void TriggerEntered(TriggerEnterCollider sender, Collider other);
public delegate void TriggerExited(TriggerEnterCollider sender, Collider other);
public delegate void TriggerStayed(TriggerEnterCollider sender, Collider other);

[AddComponentMenu("Metablast/Triggers/Components/Trigger Collider")]
[RequireComponent(typeof(Collider))]
public class TriggerEnterCollider : MonoBehaviour
{
    private static Dictionary<string, HashSet<TriggerEnterCollider>> _triggerCollidersBySet = new Dictionary<string, HashSet<TriggerEnterCollider>>();

    public static readonly EventElementCollection<string, TriggerEnterCollider, Collider> TriggerSetEnter = new EventElementCollection<string, TriggerEnterCollider, Collider>();
    public static readonly EventElementCollection<string, TriggerEnterCollider, Collider> TriggerSetStay = new EventElementCollection<string, TriggerEnterCollider, Collider>();
    public static readonly EventElementCollection<string, TriggerEnterCollider, Collider> TriggerSetExit = new EventElementCollection<string, TriggerEnterCollider, Collider>();

    public event TriggerEntered OnTriggerEntered;
    public event TriggerExited OnTriggerExited;
    public event TriggerStayed OnTriggerStayed;

#pragma warning disable 0067, 0649
    [SerializeField]
    private string _triggerSet;
#pragma warning restore 0067, 0649

    private List<Collider> _tempCopy = new List<Collider>();
    private Dictionary<int, Collider> _collisions = new Dictionary<int, Collider>();

    public bool IsInTriggerSet
    {
        get { return !string.IsNullOrEmpty(_triggerSet); }
    }

    void Start()
    {
        if (!GetComponent<Collider>().isTrigger)
        {
            DebugFormatter.LogWarning(this, "Collider is not marked as trigger. Events will not be triggered by this collider!");
            return;
        }
    }

    void OnEnable()
    {
        if (IsInTriggerSet)
        {
            HashSet<TriggerEnterCollider> colliderSet;
            if (!_triggerCollidersBySet.TryGetValue(_triggerSet, out colliderSet))
            {
                colliderSet = new HashSet<TriggerEnterCollider>();
                _triggerCollidersBySet.Add(_triggerSet, colliderSet);
            }

            colliderSet.Add(this);
        }
    }

    void OnDisable()
    {
        if (IsInTriggerSet && _triggerCollidersBySet.ContainsKey(_triggerSet))
        {
            _triggerCollidersBySet[_triggerSet].Remove(this);
        }

        _tempCopy.Clear();
        foreach (Collider other in _collisions.Values)
        {
            if (!other)
            {
                continue;
            }
            _tempCopy.Add(other);
        }

        foreach (Collider other in _tempCopy)
        {
            OnTriggerExit(other);
        }
        _tempCopy.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_collisions.ContainsKey(other.GetInstanceID()))
        {
            _collisions.Add(other.GetInstanceID(), other);
        }

        if (enabled && OnTriggerEntered != null)
        {
            DisableColliderComponent disableColliderComponent = other.GetComponent<DisableColliderComponent>();
            if (disableColliderComponent)
            {
                disableColliderComponent.ColliderDisabled += disableColliderComponent_ColliderDisabled;
            }
            OnTriggerEntered(this, other);
        }

        if (enabled && IsInTriggerSet)
        {
            TriggerSetEnter[_triggerSet].Dispatch(this, other);
        }
    }

    void disableColliderComponent_ColliderDisabled(DisableColliderComponent sender, Collider collider)
    {
        sender.ColliderDisabled -= disableColliderComponent_ColliderDisabled;
        sender.ColliderEnabled += sender_ColliderEnabled;
        OnTriggerExit(collider);
    }

    void sender_ColliderEnabled(DisableColliderComponent sender, Collider other)
    {
        Rigidbody rigidbody = other.attachedRigidbody ?? this.GetComponent<Rigidbody>();
        rigidbody.WakeUp();
    }

    void OnTriggerExit(Collider other)
    {
        _collisions.Remove(other.GetInstanceID());
        if (enabled && OnTriggerExited != null)
        {
            OnTriggerExited(this, other);
        }

        if (enabled && IsInTriggerSet)
        {
            TriggerSetExit[_triggerSet].Dispatch(this, other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (enabled && OnTriggerStayed != null)
        {
            OnTriggerStayed(this, other);
        }

        if (enabled && IsInTriggerSet)
        {
            TriggerSetStay[_triggerSet].Dispatch(this, other);
        }
    }
}

