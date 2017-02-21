using System.Linq;
using UnityEngine;

[Trigger(Description = @"Invoked every frame an object with a tag matching one of the tags in the given list is colliding with the given TriggerEnterCollider.", DisplayPath = "Collision")]
[AddComponentMenu("Metablast/Triggers/Events/Regions/Trigger Stay Event")]
public class OnTriggerStayEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("If an object collides with the given trigger collider, this event will only be invoked if the tag of the object matches one of these.")]
    private string[] _triggeringTags;

    [SerializeField]
    [Infobox("The trigger collider for which this event will be invoked.")]
    private TriggerEnterCollider _triggerCollider;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        _triggerCollider.OnTriggerStayed += OnTriggerStayEvent_OnTriggerStayed;
    }

    void OnDestroy()
    {
        _triggerCollider.OnTriggerStayed -= OnTriggerStayEvent_OnTriggerStayed;
    }

    void OnTriggerStayEvent_OnTriggerStayed(TriggerEnterCollider sender, Collider other)
    {
        if (_triggeringTags.Contains(other.tag))
        {
            TriggerEvent();
        }
    }
}

