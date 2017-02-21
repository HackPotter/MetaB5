using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public delegate void PlayerGrabbedObjectHandler(GrabbableObject grabbedObject);

[Serializable]
public class ToolEnabledEvent : UnityEvent { }

public class ImpulseBeamGrabFunction : MonoBehaviour, IFunction
{
    private enum ImpulseGunState
    {
        Idle,
        Grabbing,
        Holding,
        Throwing,
    }

    public KeyCode ShortcutKey
    {
        get { return _shortcutKey; }
    }

    public event PlayerGrabbedObjectHandler PlayerGrabbedObject;
    public event PlayerGrabbedObjectHandler PlayerDroppedObject;

#pragma warning disable 0067, 0649
    [SerializeField]
    private KeyCode _shortcutKey;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private float _maxGrabDistance;
    [SerializeField]
    private float _holdingDistanceThreshold;
    [SerializeField]
    private float _holdingForce;
    [SerializeField]
    private float _pullForce;
    [SerializeField]
    private float _angularDrag;
    [SerializeField]
    private float _drag;
    [SerializeField]
    private Vector3 _targetOffset;
    [SerializeField]
    private float _maximumThrowForce;
    [SerializeField]
    private float _throwingTime;

    [SerializeField]
    private ToolEnabledEvent _toolEnabled;
    [SerializeField]
    private ToolEnabledEvent _toolDisabled;
#pragma warning restore 0067, 0649

    [SerializeField]
    private GrabbableObject _grabbedObject;

    private bool _originalGravity;
    private float _originalAngularDrag;
    private float _originalDrag;
    private float _throwingStartTime;

    private ControlServices _controlServices;
    private CameraControllerInterface _cameraController;
    private ImpulseGunState _state;
    private HoldObjectFunction _holdObjectFunction;

    private ScannableObject _highlighted;

    void Awake()
    {
        _layerMask = new LayerMask();
        _layerMask = 1 << LayerMask.NameToLayer("GrabbableObject");
        _controlServices = GetComponent<ControlServices>();
        _cameraController = _controlServices.ControlledCamera;
        _holdObjectFunction = GetComponent<HoldObjectFunction>();

        _state = ImpulseGunState.Idle;

        GameContext.Instance.Player.ImpulseBeamTool = this;
    }

    void OnEnable()
    {
        _toolEnabled.Invoke();
    }

    void OnDisable()
    {
        _toolDisabled.Invoke();
        UngrabObject();
        DisableScannedObject();
    }

    void Update()
    {
        switch (_state)
        {
            case ImpulseGunState.Idle:
                UpdateIdle();
                break;
            case ImpulseGunState.Grabbing:
                UpdateGrabbing();
                break;
            case ImpulseGunState.Holding:
                UpdateHolding();
                break;
            case ImpulseGunState.Throwing:
                UpdateThrowing();
                break;
        }
    }

    void UpdateIdle()
    {
        if (!_cameraController.Camera)
        {
            return;
        }

        Ray ray = _cameraController.Camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, _maxGrabDistance, _layerMask))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            GrabbableObject grabbedObject = hitObject.GetComponent<GrabbableObject>();
            ScannableObject scannable = hitObject.GetComponent<ScannableObject>();

            if (scannable != _highlighted)
            {
                DisableScannedObject();
                EnableScannedObject(scannable);
            }
            if (!grabbedObject)
            {
                DisableScannedObject();
            }

            if (grabbedObject && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("");
                GrabObject(grabbedObject, hitInfo.point);
                _state = ImpulseGunState.Grabbing;
            }
        }
        else
        {
            DisableScannedObject();
        }
    }

    void UpdateGrabbing()
    {
        if (!_grabbedObject || !_grabbedObject.gameObject.activeInHierarchy)
        {
			UngrabObject();
            _state = ImpulseGunState.Idle;
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            UngrabObject();
            return;
        }

        Vector3 offset = enabled ? _cameraController.Camera.transform.forward * 3 : transform.forward * 3;
        Vector3 targetPosition = transform.TransformPoint(_targetOffset) + offset;
        Vector3 currentPosition = _grabbedObject.GetComponent<Rigidbody>().worldCenterOfMass;
        float distance = Vector3.Distance(targetPosition, currentPosition);

        if (distance < _holdingDistanceThreshold)
        {
            HoldObject(_grabbedObject);
            _state = ImpulseGunState.Holding;
        }
        Vector3 pullForce = (targetPosition - currentPosition).normalized;
        Debug.DrawLine(targetPosition, currentPosition, Color.green);
        _grabbedObject.GetComponent<Rigidbody>().AddForce(pullForce * _pullForce, ForceMode.Impulse);

    }

    void UpdateHolding()
    {
        if (!_grabbedObject || !_grabbedObject.gameObject.activeInHierarchy)
        {
			UngrabObject();
            _state = ImpulseGunState.Idle;
        }
        if (_highlighted != null)
        {
            MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("Move bioship or Left click to throw " + _highlighted.BiologEntry);
        }
        else
        {
            MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("Move bioship or Left click to throw object");
        }

        if (GameState.Instance.PauseLevel == PauseLevel.Unpaused && _grabbedObject != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                ThrowObject();
                MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("");
            }
        }
    }

    void UpdateThrowing()
    {
		if (!_grabbedObject || !_grabbedObject.gameObject.activeInHierarchy)
        {
			UngrabObject();
            _state = ImpulseGunState.Idle;
        }
        if (Time.time - _throwingStartTime >= _throwingTime || Input.GetMouseButtonUp(0))
        {
            Vector3 direction = _cameraController.Camera.transform.forward;

            Vector3 force = Vector3.zero;

            float power = (Time.time - _throwingStartTime) / _throwingTime;
            power *= power;

            Rigidbody grabbedObject = _grabbedObject.GetComponent<Rigidbody>();
            UngrabObject();

            grabbedObject.AddForce(direction * _maximumThrowForce * power, ForceMode.Impulse);
        }
    }

    private void HoldObject(GrabbableObject grabbedObject)
    {
        if (PlayerGrabbedObject != null)
        {
            PlayerGrabbedObject(grabbedObject);
        }
        _holdObjectFunction.HoldObject(grabbedObject, _targetOffset, _holdingForce);
    }

    private void GrabObject(GrabbableObject grabbableObject, Vector3 grabPoint)
    {
        _grabbedObject = grabbableObject;

        _originalGravity = _grabbedObject.GetComponent<Rigidbody>().useGravity;
        _originalAngularDrag = _grabbedObject.GetComponent<Rigidbody>().angularDrag;
        _originalDrag = _grabbedObject.GetComponent<Rigidbody>().drag;


        _grabbedObject.GetComponent<Rigidbody>().useGravity = false;
        _grabbedObject.GetComponent<Rigidbody>().angularDrag = _angularDrag;
        _grabbedObject.GetComponent<Rigidbody>().drag = _drag;

        AnalyticsLogger.Instance.AddLogEntry(new ObjectGrabbedLogEntry(GameContext.Instance.Player.UserGuid, _grabbedObject));
    }

    private void UngrabObject()
    {
        if (_grabbedObject)
        {
            AnalyticsLogger.Instance.AddLogEntry(new ObjectDroppedLogEntry(GameContext.Instance.Player.UserGuid, _grabbedObject));
            _grabbedObject.GetComponent<Rigidbody>().useGravity = _originalGravity;
            _grabbedObject.GetComponent<Rigidbody>().drag = _originalDrag;
            _grabbedObject.GetComponent<Rigidbody>().angularDrag = _originalAngularDrag;
            
            _state = ImpulseGunState.Idle;

            if (PlayerDroppedObject != null)
            {
                PlayerDroppedObject(_grabbedObject);
            }
            _grabbedObject = null;
            _holdObjectFunction.ReleaseObject();
        }
    }

    private void ThrowObject()
    {
        if (enabled)
        {
            _throwingStartTime = Time.time;
            _state = ImpulseGunState.Throwing;
        }
    }

    public void OnAcquiredControl()
    {
        this.enabled = true;
    }

    public void OnLostControl()
    {
        this.enabled = false;
		if (_grabbedObject)
		{
			UngrabObject();
		}
    }

    private void EnableScannedObject(ScannableObject scannable)
    {
        if (scannable)
        {
            scannable.enabled = true;
            _highlighted = scannable;
            MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("Click to grab " + _highlighted.BiologEntry);
        }
    }

    private void DisableScannedObject()
    {
        if (_highlighted)
        {
            _highlighted.enabled = false;
            _highlighted = null;
            MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("");
        }
    }

    private void DoScan()
    {
        Camera cam = _controlServices.ControlledCamera.Camera;
        if (!cam)
        {
            DisableScannedObject();
            return;
        }

        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (!Physics.Raycast(r, out hitInfo, _maxGrabDistance))
        {
            DisableScannedObject();
            return;
        }

        GameObject hitObject = hitInfo.collider.gameObject;
        ScannableObject scannable = hitObject.GetComponent<ScannableObject>();
        if (scannable != _highlighted)
        {
            DisableScannedObject();
            EnableScannedObject(scannable);
            return;
        }
    }
}

