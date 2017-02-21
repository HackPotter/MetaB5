using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Trigger(Description = @"Invoked when an object with a tag matching one of the tags in the given list enters the given TriggerEnterCollider.
If DisableColliderAfterTrigger is enabled, the TriggerEnterCollider will be disabled.", DisplayPath = "Collision")]
[AddComponentMenu("Metablast/Triggers/Events/Regions/Trigger Enter Event")]
public class OnTriggerEnterEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("If an object collides with the given trigger collider, this event will only be invoked if the tag of the object matches one of these.")]
    private string[] _triggeringTags;

    [SerializeField]
    [Infobox("The trigger collider for which this event will be invoked.")]
    private TriggerEnterCollider _triggerCollider;

    [SerializeField]
    [Infobox("If true, the trigger collider will be disabled after this is invoked.")]
    private bool _disableColliderAfterTrigger;
#pragma warning restore 0067, 0649

    private GameObject _triggeringObject;

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
        if (!_triggerCollider)
        {
            DebugFormatter.LogError(this, "Trigger Collider cannot be null.");
            this.enabled = false;
            return;
        }

        if (_triggeringTags.Length == 0)
        {
            DebugFormatter.LogError(this, "Triggering tags is size 0. This event will never be invoked!");
        }

        _triggerCollider.OnTriggerEntered += OnTriggerEnterEvent_OnTriggerEntered;
    }

    void OnDestroy()
    {
        _triggerCollider.OnTriggerEntered -= OnTriggerEnterEvent_OnTriggerEntered; ;
    }

    void OnTriggerEnterEvent_OnTriggerEntered(TriggerEnterCollider sender, Collider other)
    {
        if (gameObject.activeSelf && _triggeringTags.Contains(other.tag))
        {
            if (_disableColliderAfterTrigger)
            {
                sender.enabled = false;
            }
            _triggeringObject = other.gameObject;
            TriggerEvent();
            _triggeringObject = null;
        }
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Triggering Game Object", _triggeringObject);
        context.AddLocal("Trigger Collider", _triggerCollider);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
        {
            new OutputParameterDeclaration()
            {
                Name = "Triggering Game Object",
                Type = typeof(GameObject)
            },
            new OutputParameterDeclaration()
            {
                Name = "Trigger Collider",
                Type = typeof(TriggerEnterCollider)
            }
        };

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }
}

