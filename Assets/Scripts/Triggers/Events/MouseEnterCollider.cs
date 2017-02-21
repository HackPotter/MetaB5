using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Trigger(Description = "Invoked after the mouse has started hovering on top of an object in the given layer with a tag in the given list of tags within the specified maximum distance.",
    DisplayPath = "Input")]
[AddComponentMenu("Metablast/Triggers/Events/Input/Mouse Enter Collider")]
public class MouseEnterCollider : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("If the tag of the object being hovered over is not contained in this list, then the event will not be invoked.")]
    private string[] _triggeringTags;

    [SerializeField]
    [Infobox("If an object is not in this layer, then it will not be considered.")]
    private LayerMask _layerMask;

    [SerializeField]
    [Infobox("The maximum distance an object can be away from the camera before it will not be considered.")]
    private float _maxDistance;
#pragma warning restore 0067, 0649

    private GameObject _lastObject;
    private GameObject _triggeringObject;

    void FixedUpdate()
    {
        if (gameObject.activeSelf && Camera.main && _maxDistance > 0f)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(r, out hitInfo, _maxDistance, _layerMask) && _triggeringTags.Contains(hitInfo.collider.tag))
            {
                if (_lastObject != hitInfo.collider.gameObject)
                {
                    _triggeringObject = hitInfo.collider.gameObject;
                    TriggerEvent();
                    _triggeringObject = null;
                    _lastObject = hitInfo.collider.gameObject;
                }
            }
            else
            {
                _lastObject = null;
            }
        }
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Entered Game Object", _triggeringObject);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
        {
            new OutputParameterDeclaration()
            {
                Name = "Entered Game Object",
                Description = "The object that the mouse has hovered over.",
                Type = typeof(GameObject)
            }
        };

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }
}

