using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Trigger(Description = "Invoked every frame an object with a tag contained in TriggeringTags is colliding a trigger in the provided set name.\nTo add a trigger to a trigger set, specify the name of the trigger set on the TriggerEnterCollider object.", DisplayPath = "Collision")]
public class AnyTriggerStay : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("If an object is inside the given TriggerEnterCollider, this event will be invoked if its tag is contained in this list.")]
    private string[] _triggeringTags;
    [SerializeField]
    [Infobox("This event will be invoked every frame if an object is inside any TriggerEnterCollider contained in the trigger set by this name.")]
    private string _triggerSet;
#pragma warning restore 0067, 0649

    private TriggerEnterCollider _trigger;
    private GameObject _triggeringObject;

    private void OnEnable()
    {
        TriggerEnterCollider.TriggerSetStay[_triggerSet] += TriggerSetEnterHandler;
    }

    private void OnDisable()
    {
        TriggerEnterCollider.TriggerSetStay[_triggerSet] -= TriggerSetEnterHandler;
    }

    private void TriggerSetEnterHandler(TriggerEnterCollider trigger, Collider collider)
    {
        if (gameObject.activeSelf && _triggeringTags.Contains(collider.tag))
        {
            _trigger = trigger;
            _triggeringObject = collider.gameObject;

            TriggerEvent();
        }
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Trigger Collider", _trigger);
        context.AddLocal("Triggering Game Object", _triggeringObject);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
        {
            new OutputParameterDeclaration()
            {
                Name = "Trigger Collider",
                Description = "The trigger collider that the triggering object is colliding with.",
                Type = typeof(TriggerEnterCollider)
            },
            new OutputParameterDeclaration()
            {
                Name = "Triggering Game Object",
                Description = "The object that is colliding with the trigger collider.",
                Type = typeof(GameObject)
            }
        };

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }
}
