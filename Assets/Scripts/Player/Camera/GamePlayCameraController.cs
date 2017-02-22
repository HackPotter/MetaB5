using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// Controls the camera for our player.
/// </summary>
[RequireComponent(typeof(ControlServices))]
public class GamePlayCameraController : BaseCameraController
{
    public override void OnAcquiredControl()
    {
        if (!_cam)
        {
            _cam = GetComponent<ControlServices>().ControlledCamera;
        }

        _cam.CameraOffset = _cameraOffset;
        _cam.CameraCollisionMask = _cameraCollisionMask;
        _cam.OrbitDistance = _lastOrbitDistance;
        AdjustOrbitDistance(0);
        enabled = true;
    }

    private float _lastOrbitDistance;

    public override void OnLostControl()
    {
        _lastOrbitDistance = _cam.OrbitDistance;
        enabled = false;
    }

    // Services
    private CameraControllerInterface _cam;
    private ControlServices _controlServices;

#pragma warning disable 0649
    [SerializeField]
    private Vector3 _cameraOffset;
    [SerializeField]
    private LayerMask _cameraCollisionMask;
    [SerializeField]
    private float _minimumFollowingDistance = 0.25f;
    [SerializeField]
    private float _maximumFollowingDistance = 5f;
    [SerializeField]
    private float _orbitDeltaMultiplier = 10f;
    [SerializeField]
    private float _arcDeltaMultiplier = 5f;
    [SerializeField]
    private float _keyboardTurningRate = 250.0f;
#pragma warning restore 0649

    private bool _controlsEnabled = true;

    // Specific Parameters
    public float TurningRate = 1.0f;

    void Start()
    {
        _controlServices = GetComponent<ControlServices>();
        _cam = _controlServices.ControlledCamera;

        _lastOrbitDistance = (_minimumFollowingDistance + _maximumFollowingDistance) * 0.5f;
        GameState.Instance.OnPauseLevelChanged += new PauseLevelChanged(Instance_OnPauseLevelChanged);

        OnAcquiredControl();
    }

    void Instance_OnPauseLevelChanged(PauseLevel pauseLevel)
    {
        _controlsEnabled = pauseLevel == PauseLevel.Unpaused;
    }

    void FixedUpdate()
    {
        if (GameState.Instance.PauseLevel != PauseLevel.Unpaused)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        if (!EventSystem.current.IsPointerOverGameObject() && _controlsEnabled && Input.GetMouseButton(1))
        {
            float turn = Input.GetAxis("Mouse X");
            _cam.TargetForward = Quaternion.AngleAxis(TurningRate * turn, Vector3.up) * _cam.TargetForward;

            Debug.DrawLine(transform.position + 2 * Vector3.up, transform.position + 2 * Vector3.up + _cam.TargetForward * 3, Color.magenta);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        
        if (Input.GetKey(KeyCode.A))
        {
            _cam.TargetForward = Quaternion.AngleAxis(-_keyboardTurningRate * Time.deltaTime, Vector3.up) * _cam.TargetForward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _cam.TargetForward = Quaternion.AngleAxis(_keyboardTurningRate * Time.deltaTime, Vector3.up) * _cam.TargetForward;
        }

        // Pan in or out.
        float orbitDelta = Input.GetAxis("Mouse ScrollWheel");
        if (orbitDelta != 0)
        {
            AdjustOrbitDistance(_orbitDeltaMultiplier * orbitDelta);
        }

        if (!EventSystem.current.IsPointerOverGameObject() && _controlsEnabled && Input.GetMouseButton(1))
        {
            _cam.Arc -= Input.GetAxis("Mouse Y") * _arcDeltaMultiplier;
            //_cam.Arc = ClampAngle(_cam.Arc - Input.GetAxis("Mouse Y") * _arcDeltaMultiplier, -90f, 90f);
        }
    }

    private void AdjustOrbitDistance(float panChange)
    {
        _cam.OrbitDistance -= panChange;

        if (_cam.OrbitDistance > _maximumFollowingDistance)
            _cam.OrbitDistance = _maximumFollowingDistance;
        if (_cam.OrbitDistance < _minimumFollowingDistance)
            _cam.OrbitDistance = _minimumFollowingDistance;
    }

    private float ClampAngle(float ang, float min, float max)
    {
        if (ang > 180) ang = ang - 360;
        ang = Mathf.Clamp(ang, min, max);
        if (ang < 0) ang = 360 + ang;
        return ang;
    }
}