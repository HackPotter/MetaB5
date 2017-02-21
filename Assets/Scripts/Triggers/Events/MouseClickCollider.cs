using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[Trigger(Description = "Invoked when an object in the given layer with a tag in the given list of tags within the given maximum distance has been clicked.", DisplayPath = "Input")]
[AddComponentMenu("Metablast/Triggers/Events/Input/Mouse Click Collider")]
public class MouseClickCollider : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The mouse button for which this event will be invoked.")]
    private MouseButton _mouseButton;

    [SerializeField]
    [Infobox("Whether or not to ignore the UI.")]
    private MouseClickTargets _mouseTargets;

    [SerializeField]
    [Infobox("The type of mouse click for which this event will be invoked.")]
    private MouseClickType _mouseClickType;

    [SerializeField]
    [Infobox("When an object is clicked, if its tag is in this list, then this event will be invoked.")]
    private string[] _triggeringTags;

    [SerializeField]
    [Infobox("If an object is not in the given layer, then it will not be considered.")]
    private LayerMask _layerMask;

    [SerializeField]
    [Infobox("The maximum distance an object can be from the camera to be considered.")]
    private float _maxDistance;
#pragma warning restore 0067, 0649

    private bool _clicked = false;
    private GameObject _triggeringObject;

    void Update()
    {
        switch (_mouseClickType)
        {
            case MouseClickType.MouseDown:
                if (Input.GetMouseButtonDown((int)_mouseButton) && (!EventSystem.current.IsPointerOverGameObject() || _mouseTargets == MouseClickTargets.Everything))
                {
                    _clicked = true;
                }
                break;
            case MouseClickType.MouseUp:
                if (Input.GetMouseButtonUp((int)_mouseButton) && (!EventSystem.current.IsPointerOverGameObject() || _mouseTargets == MouseClickTargets.Everything))
                {
                    _clicked = true;
                }
                break;
            case MouseClickType.MouseHold:
                if (Input.GetMouseButton((int)_mouseButton) && (!EventSystem.current.IsPointerOverGameObject() || _mouseTargets == MouseClickTargets.Everything))
                {
                    _clicked = true;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (_clicked)
        {
            if (gameObject.activeSelf && Camera.main && _maxDistance > 0f)
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(r, out hitInfo, _maxDistance, _layerMask) && _triggeringTags.Contains(hitInfo.collider.tag))
                {
                    _triggeringObject = hitInfo.collider.gameObject;
                    TriggerEvent();
                    _triggeringObject = null;
                }
            }
            _clicked = false;
        }
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Clicked Game Object", _triggeringObject);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
        {
            new OutputParameterDeclaration()
            {
                Name = "Clicked Game Object",
                Description = "The object that has been clicked.",
                Type = typeof(GameObject)
            }
        };

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }
}

