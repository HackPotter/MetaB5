using UnityEngine;


[RequireComponent(typeof(ControlServices))]
[RequireComponent(typeof(Rigidbody))]
public class VehicleController3D : MonoBehaviour
{
    // Parameters
#pragma warning disable 0067, 0649
    [SerializeField]
    private float _forwardThrust;
    [SerializeField]
    private float _forwardBoostThrust;
    [SerializeField]
    private float _reverseThrust;
    [SerializeField]
    private float _reverseBoostThrust;
    [SerializeField]
    private float _verticalThrust;
    [SerializeField]
    private float _verticalBoostThrust;
    [SerializeField]
    private float _cameraAngleOffset;
    [SerializeField]
    private float _atpBoostDrainRate = 1.0f;
    [SerializeField]
    private float _correctiveRotationForce;
#pragma warning restore 0067, 0649

    // Transient State
    private Vector3 _currentThrustLevel;

    // Services
    private ControlServices _controlServices;

    private float _lastLogTime = 0f;
    private float _logFrequency = 3f;

    void Awake()
    {
        _controlServices = GetComponent<ControlServices>();
    }

    void Update()
    {
        if (GameState.Instance.PauseLevel == PauseLevel.Unpaused)
        {
            Vector3 thrust = Vector3.zero;
            bool boosting = Input.GetKey(KeyCode.LeftShift) && GameContext.Instance.Player.ATP > _atpBoostDrainRate * Time.deltaTime;

            if (boosting)
            {
                GameContext.Instance.Player.ATP -= _atpBoostDrainRate * Time.deltaTime;
            }

            int thrustDirections = 0;

            if (Input.GetKey(KeyCode.W))
            {
                thrust += transform.forward * (boosting ? _forwardBoostThrust : _forwardThrust);
                thrustDirections++;
            }
            if (Input.GetKey(KeyCode.S))
            {
                thrust += transform.forward * (boosting ? -_reverseBoostThrust : -_reverseThrust);
                thrustDirections++;
            }
            if (Input.GetKey(KeyCode.E))
            {
                thrust += transform.up * (boosting ? _verticalBoostThrust : _verticalThrust);
                thrustDirections++;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                thrust += transform.up * (boosting ? -_verticalBoostThrust : -_verticalThrust);
                thrustDirections++;
            }

            if (thrustDirections != 0)
            {
                thrust = thrust / thrustDirections;
            }

            _currentThrustLevel = thrust;
        }
        else
        {
            _currentThrustLevel = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        Vector3 targetForward = _controlServices.ControlledCamera.Camera.transform.forward;
        targetForward = Quaternion.AngleAxis(_cameraAngleOffset, _controlServices.ControlledCamera.Camera.transform.right) * targetForward;
        Vector3 targetUp = _controlServices.ControlledCamera.Camera.transform.up;
        targetUp = Quaternion.AngleAxis(_cameraAngleOffset, _controlServices.ControlledCamera.Camera.transform.right) * targetUp;

        Vector3 z = Vector3.Cross(transform.forward, targetForward);
        Vector3 y = Vector3.Cross(transform.up, targetUp);

        float zAngleDiff = Vector3.Angle(transform.forward, targetForward);
        float yAngleDiff = Vector3.Angle(transform.up, targetUp);


        Vector3 torque = (z * zAngleDiff + y * yAngleDiff) * _correctiveRotationForce;

        GetComponent<Rigidbody>().AddTorque(torque);
        GetComponent<Rigidbody>().AddForce(_currentThrustLevel);

        if (Time.realtimeSinceStartup - _lastLogTime > _logFrequency)
        {
            AnalyticsLogger.Instance.AddLogEntry(new PlayerPositionUpdateLogEntry(GameContext.Instance.Player.UserGuid, transform.position));
            _lastLogTime = Time.realtimeSinceStartup;
        }
    }

    void ApplyOrientationRightingForce()
    {
        Vector3 targetUp = Vector3.Cross(transform.forward, _controlServices.ControlledCamera.Camera.transform.right);
        float angleDiff = Vector3.Angle(transform.up, targetUp);
        Vector3 cross = Vector3.Cross(transform.up, targetUp);
        GetComponent<Rigidbody>().AddTorque(cross * angleDiff * _correctiveRotationForce);
    }
}

