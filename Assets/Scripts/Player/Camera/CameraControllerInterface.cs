using System.Linq;
using UnityEngine;

// Pull out camera logic and put here. Have CameraControllers use this.
public class CameraControllerInterface : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject _cameraRoot;
#pragma warning restore 0067, 0649

    private Camera _cam;

    private bool _controlsEnabled = true;
    private ControlServices _controlServices;

    public Camera Camera
    {
        get { return _cam; }
    }

    public Vector3 TargetForward
    {
        get;
        set;
    }

    public float OrbitDistance
    {
        get;
        set;
    }

    public float Arc
    {
        get { return _targetArc; }
        set
        {
            _targetArc = value;
            _targetArc = Mathf.Clamp(_targetArc, -90f, 90f);
            //_targetArc = ClampAngle(_targetArc, -90f, 90f);
        }
    }

    public Vector3 CameraOffset
    {
        get;
        set;
    }

    public LayerMask CameraCollisionMask
    {
        get { return _cameraCollisionMask; }
        set { _cameraCollisionMask = value; }
    }

    // State
    private Vector3 _cameraOffset;
    private LayerMask _cameraCollisionMask;
    private float _orbitDistance = 3;
    private float _targetArc;
    private float _arc;
    private Quaternion _targetRotation;
    private Vector3 _targetPosition;
    private Vector3 _actualForward;

    void Awake()
    {
        _cam = _cameraRoot.GetComponentsInChildren<Camera>().First((c) => c.gameObject.activeInHierarchy);
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        _actualForward = forward;
        TargetForward = _actualForward;
    }

    void Start()
    {
        _controlServices = GetComponent<ControlServices>();
        GameState.Instance.OnPauseLevelChanged += new PauseLevelChanged(Instance_OnPauseLevelChanged);
    }

    void Instance_OnPauseLevelChanged(PauseLevel pauseLevel)
    {
        _controlsEnabled = pauseLevel == PauseLevel.Unpaused;
    }

    void FixedUpdate()
    {
        _actualForward = Vector3.Slerp(_actualForward, TargetForward, 0.1f);

        _orbitDistance = Mathf.Lerp(_orbitDistance, OrbitDistance, 0.1f);
        _cameraOffset = Vector3.Lerp(_cameraOffset, CameraOffset, 0.1f);
        _arc = Mathf.Lerp(_arc, Arc, 0.1f);
    }

    private bool _initialized = false;

    void LateUpdate()
    {
        // Interpolate parameters:

        if (!_initialized)
        {

        }

        // Move the camera to the focus point and rotate it.
        _targetPosition = transform.TransformPoint(_cameraOffset);
        _targetRotation = Quaternion.LookRotation(_actualForward) * Quaternion.AngleAxis(_arc, Vector3.right);

        RaycastHit info;
        float compensatedRadius = _orbitDistance;

        float nearPlaneSize = _cam.GetComponent<Camera>().nearClipPlane / Mathf.Cos(Mathf.Deg2Rad * _cam.GetComponent<Camera>().fieldOfView * 0.5f);
        if (Physics.SphereCast(_targetPosition, nearPlaneSize * 2, -_cam.transform.forward, out info, _orbitDistance, _cameraCollisionMask))
        {
            Debug.DrawLine(info.point, info.normal * 5 + info.point, Color.magenta);
            compensatedRadius = Vector3.Distance(info.point, _targetPosition);
        }

        // Push back out along the "back" vector of the camera for a distance radius.
        _targetPosition += _targetRotation * (compensatedRadius * Vector3.back);

        _cam.transform.rotation = _targetRotation;
        //_cam.transform.rotation = Quaternion.Euler(ClampAngle(_cam.transform.rotation.eulerAngles.x, -90f, 90f), _cam.transform.eulerAngles.y, _cam.transform.eulerAngles.z);
        _cam.transform.position = _targetPosition;
    }

    private float ClampAngle(float ang, float min, float max)
    {
        if (ang > 180) ang = ang - 360;
        ang = Mathf.Clamp(ang, min, max);
        if (ang < 0) ang = 360 + ang;
        return ang;
    }

    void OnDrawGizmos()
    {
        if (_cam)
        {
            float nearPlaneSize = _cam.nearClipPlane / Mathf.Cos(Mathf.Deg2Rad * _cam.fieldOfView * 0.5f);
            Gizmos.DrawSphere(_cam.transform.position, nearPlaneSize);
        }
    }
}

