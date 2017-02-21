using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Trigger(Description = @"Invoked when an object with a tag matching one of the tags in the given list exits the given TriggerEnterCollider.", DisplayPath = "Collision")]
[AddComponentMenu("Metablast/Triggers/Events/Regions/Trigger Exit Event")]
public class OnTriggerExitEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("If an object collides with the given trigger collider, this event will only be invoked if the tag of the object matches one of these.")]
    private string[] _triggeringTags;

    [SerializeField]
    [Infobox("The trigger collider for which this event will be invoked.")]
    private TriggerEnterCollider _triggerCollider;
#pragma warning restore 0067, 0649

    private GameObject _triggeringObject;

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

        _triggerCollider.OnTriggerExited += OnTriggerExitEvent_OnTriggerExited;
    }

    void OnDestroy()
    {
        _triggerCollider.OnTriggerExited -= OnTriggerExitEvent_OnTriggerExited;
    }

    void OnTriggerExitEvent_OnTriggerExited(TriggerEnterCollider sender, Collider other)
    {
        if (_triggeringTags.Contains(other.tag))
        {
            _triggeringObject = other.gameObject;
            TriggerEvent();
            _triggeringObject = null;
        }
    }

    private List<OutputParameterDeclaration> _outputParameters = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name="Triggering Game Object",
            Type=typeof(GameObject)
        },
        new OutputParameterDeclaration()
        {
            Name="Trigger Collider",
            Type=typeof(TriggerEnterCollider)
        },
    };

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _outputParameters;
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Triggering Game Object", _triggeringObject);
        context.AddLocal("Trigger Collider", _triggerCollider);
    }
}

