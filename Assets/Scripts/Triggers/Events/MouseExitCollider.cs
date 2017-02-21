using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Trigger(Description = "Invoked the first frame the user has stopped hovering the mouse over an object with a tag in the given list of tags, in the given layer, and within the specified maximum distance.", DisplayPath = "Input")]
[AddComponentMenu("Metablast/Triggers/Events/Input/Mouse Exit Collider")]
public class MouseExitCollider : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("If the tag of the object the mouse has stopped hovering over is not contained in this list, then the event will not be invoked.")]
    private string[] _triggeringTags;

    [SerializeField]
    [Infobox("If an object is not in this layer, then it will not be considered.")]
    private LayerMask _layerMask;

    [SerializeField]
    [Infobox("The maximum distance an object can be away from the camera before it will not be considered.")]
    private float _maxDistance;
#pragma warning restore 0067, 0649

    private GameObject _lastHoverGameObject;
    private GameObject _triggeringObject;

    void FixedUpdate()
    {
        if (gameObject.activeSelf && Camera.main && _maxDistance > 0f)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(r, out hitInfo, _maxDistance, _layerMask);

            if (hit && _triggeringTags.Contains(hitInfo.collider.tag))
            {
                if (_lastHoverGameObject && _lastHoverGameObject != hitInfo.collider.gameObject)
                {
                    _triggeringObject = _lastHoverGameObject;
                    TriggerEvent();
                    _triggeringObject = null;
                }

                _lastHoverGameObject = hitInfo.collider.gameObject;
            }
            else if (_lastHoverGameObject)
            {
                _triggeringObject = _lastHoverGameObject;
                TriggerEvent();
                _triggeringObject = null;

                _lastHoverGameObject = null;
            }
        }
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Exited Game Object", _triggeringObject);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
        {
            new OutputParameterDeclaration()
            {
                Name = "Exited Game Object",
                Description = "The object that the mouse has stopped hovering over.",
                Type = typeof(GameObject)
            }
        };

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }
}

