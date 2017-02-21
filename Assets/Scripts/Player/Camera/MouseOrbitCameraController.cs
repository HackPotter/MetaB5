using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ControlServices))]
public class MouseOrbitCameraController : BaseCameraController
{
    public override void OnAcquiredControl()
    {
        if (!_cam)
        {
            _cam = GetComponent<ControlServices>().ControlledCamera;
        }

        _cam.CameraOffset = _cameraOffset;
        _cam.CameraCollisionMask = _cameraCollisionMask;
        _cam.OrbitDistance = _cameraOrbitDistance;

        enabled = true;
    }

    public override void OnLostControl()
    {
        this.enabled = false;
    }

    private ControlServices _controlServices;
    private CameraControllerInterface _cam;

#pragma warning disable 0067, 0649
    [SerializeField]
    private LayerMask _cameraCollisionMask;
    [SerializeField]
    private Vector3 _cameraOffset;
    [SerializeField]
    private float _turningRate = 1.0f;
    [SerializeField]
    private float _keyboardTurningRate = 2f;
    [SerializeField]
    private float _cameraOrbitDistance = 0.5f;
#pragma warning restore 0067, 0649

    private bool _controlsEnabled = true;

    void Awake()
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
        if (Input.GetKey(KeyCode.A))
        {
            _cam.TargetForward = Quaternion.AngleAxis(-_keyboardTurningRate * Time.deltaTime, Vector3.up) * _cam.TargetForward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _cam.TargetForward = Quaternion.AngleAxis(_keyboardTurningRate * Time.deltaTime, Vector3.up) * _cam.TargetForward;
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(1))
        {
            float turn = Input.GetAxis("Mouse X");
            _cam.TargetForward = Quaternion.AngleAxis(_turningRate * turn * Time.deltaTime, Vector3.up) * _cam.TargetForward;

            Debug.DrawLine(transform.position + 2 * Vector3.up, transform.position + 2 * Vector3.up + _cam.TargetForward * 3, Color.magenta);
            Screen.lockCursor = true;
        }
        else
        {
            Screen.lockCursor = false;
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(1))
        {
            _cam.Arc = ClampAngle(_cam.Arc - Input.GetAxis("Mouse Y") * Time.deltaTime * _turningRate, -90f, 90f);
        }
    }

    private float ClampAngle(float ang, float min, float max)
    {
        if (ang > 180) ang = ang - 360;
        ang = Mathf.Clamp(ang, min, max);
        if (ang < 0) ang = 360 + ang;
        return ang;
    }
}

