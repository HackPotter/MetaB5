#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using UnityEngine.EventSystems;

public class VehiclePlayerController : BasePlayerController
{
    public override void OnAcquiredControl()
    {
        this.enabled = true;
    }

    public override void OnLostControl()
    {
        this.enabled = false;
    }

    public float TargetSuspensionHeight = 0.5f;

    public float ForwardThrust = 1.0f;
    public float ForwardBoostThrust = 2000f;
    public float InAirThrust = 1.0f;
    public float TurningTorque = 100f;
    public float TurningDampening = 0.5f;
    public float LiftAmount = 100f;
    public float InfiniteDragVelocityThreshold = 0.5f;

    public float StaticCoefficientVelocityThreshold = 0.5f;
    public float CorrectiveRotationForce = 1f;

    public float stiffness = 1000;
    public float damping = 0.5f;

    public Vector3[] LeftOffsets;
    public Vector3[] RightOffsets;

    public float SidewaysDrag = 0.95f;
    public float ForwardDrag = 0.5f;

    public Vector3 AngularDrag;

    public LayerMask RaycastLayers;
    private Vector3 origCOM;

    public float LiftForce;
    public float LiftDuration;
    private bool _canLift;

    [SerializeField]
    private float _atpBoostDrainRate = 1.0f;

    [SerializeField]
    private float _atpLiftDrainRate = 1.0f;

    private float _currentThurstLevel;
    private ControlServices _controlServices;


    // Use this for initialization
    void Start()
    {
        _controlServices = GetComponent<ControlServices>();
        origCOM = GetComponent<Rigidbody>().centerOfMass;
        GetComponent<Rigidbody>().centerOfMass = GetComponent<Rigidbody>().centerOfMass;// -new Vector3(0, 1, 0);
        origCOM = GetComponent<Rigidbody>().centerOfMass;
        _currentThurstLevel = ForwardThrust;
    }

    void Update()
    {
        if (GameState.Instance.PauseLevel == PauseLevel.Unpaused)
        {
            if (Input.GetKey(KeyCode.LeftShift) && GameContext.Instance.Player.ATP > _atpBoostDrainRate * Time.deltaTime)
            {
                GameContext.Instance.Player.ATP -= _atpBoostDrainRate * Time.deltaTime;
                _currentThurstLevel = ForwardBoostThrust;
            }
            else
            {
                _currentThurstLevel = ForwardThrust;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = _controlServices.ControlledCamera.TargetForward;

        dir.y = 0;
        dir.Normalize();
        dir = transform.InverseTransformDirection(dir);

        Vector3 projDirForward = Vector3.Project(dir, Vector3.forward);
        Vector3 projDirRight = Vector3.Project(dir, Vector3.right);

        float thrust = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
        thrust = GameState.Instance.PauseLevel == PauseLevel.Unpaused ? thrust : 0;

        //How forward we're facing in relation to thrust.
        // 1 = Facing forward, moving forward OR facing backward, moving backward 
        // -1 = Facing forward, moving backward OR facing backward, moving forward
        float forward = projDirForward.z * thrust;
        float turning = projDirRight.x * 1;

        bool onGround = false;
        for (int i = 0; i < LeftOffsets.Length; i++)
        {
            onGround |= CalculateTreadPhysics(LeftOffsets[i],forward,turning);
        }

        for (int i = 0; i < RightOffsets.Length; i++)
        {
            onGround |= CalculateTreadPhysics(RightOffsets[i], forward, -turning);
        }

        if (onGround)
        {
            if (Vector3.Dot(Vector3.up, transform.up) > 0.75f)
            {
                _canLift = true;
            }
            if (GetComponent<Rigidbody>().velocity.magnitude < InfiniteDragVelocityThreshold)
            {
                GetComponent<Rigidbody>().drag = 400;
            }
            else
            {
                GetComponent<Rigidbody>().drag = 0;
            }
            Vector3 forwardVelocity = Vector3.Project(GetComponent<Rigidbody>().velocity, transform.forward);
            Vector3 horizontalVelocity = Vector3.Project(GetComponent<Rigidbody>().velocity, transform.right);
            GetComponent<Rigidbody>().velocity -= horizontalVelocity * SidewaysDrag;
            GetComponent<Rigidbody>().velocity -= forwardVelocity * ForwardDrag;
        }
        else
        {
            GetComponent<Rigidbody>().drag = 0;
        }

        Vector3 forwardThrustDirection = transform.forward;
        Vector3 force = forward * forwardThrustDirection;
        if (force.magnitude >= StaticCoefficientVelocityThreshold)
        {
            GetComponent<Rigidbody>().drag = 0;
            if (onGround)
            {
                GetComponent<Rigidbody>().AddForceAtPosition(_currentThurstLevel * force, GetComponent<Rigidbody>().worldCenterOfMass);
            }
            else
            {
                GetComponent<Rigidbody>().AddForceAtPosition(InAirThrust * force, GetComponent<Rigidbody>().worldCenterOfMass);
            }
        }

        GetComponent<Rigidbody>().AddTorque(TurningTorque * transform.up * (turning - TurningDampening * GetComponent<Rigidbody>().angularVelocity.y));

        if (GameState.Instance.PauseLevel == PauseLevel.Unpaused && Input.GetKey(KeyCode.Space))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up);


            //get the angle between transform.forward and target delta
            float angleDiff = Vector3.Angle(transform.up, Vector3.up);

            // get its cross product, which is the axis of rotation to
            // get from one vector to the other
            Vector3 cross = Vector3.Cross(transform.up, Vector3.up);

            // apply torque along that axis according to the magnitude of the angle.
            GetComponent<Rigidbody>().AddTorque(cross * angleDiff * CorrectiveRotationForce);

            if (_canLift)
            {
            }

            if (_canLift)
            {
                GetComponent<Rigidbody>().drag = 0;
                if (GameContext.Instance.Player.ATP > _atpLiftDrainRate * Time.deltaTime)//(Time.time - _liftStartTime < LiftDuration)
                {
                    GameContext.Instance.Player.ATP -= _atpLiftDrainRate * Time.deltaTime;
                    GetComponent<Rigidbody>().AddForce(Vector3.up * LiftForce);
                }
            }
        }
        else
        {
            _canLift = true;
        }


        GetComponent<Rigidbody>().angularVelocity = new Vector3(GetComponent<Rigidbody>().angularVelocity.x * (1 - AngularDrag.x),
                                        GetComponent<Rigidbody>().angularVelocity.y * (1 - AngularDrag.y),
                                        GetComponent<Rigidbody>().angularVelocity.z * (1 - AngularDrag.z));
    }

    private bool CalculateTreadPhysics(Vector3 suspensionStartPoint, float forwardComponent, float turningComponent)
    {
        Vector3 origin = transform.TransformPoint(suspensionStartPoint);
        Ray r = new Ray(origin, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, TargetSuspensionHeight, RaycastLayers))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) < 0.5f)
            {
                return false;
            }
            float compression = Vector3.Distance(origin - transform.up * TargetSuspensionHeight, hit.point);

            Vector3 pointVel = GetComponent<Rigidbody>().GetPointVelocity(origin);
            pointVel = Vector3.Project(pointVel, transform.up);
            Vector3 force = stiffness * compression * transform.up - damping * pointVel * GetComponent<Rigidbody>().mass;

            GetComponent<Rigidbody>().AddForceAtPosition(force, origin);
            return true;
        }
        Debug.DrawLine(r.origin, r.origin + r.direction * TargetSuspensionHeight);
        return false;
        
    }
    //EventSystem.current.IsPointerOverGameObject()
    void LateUpdate()
    {
        if (GameState.Instance.PauseLevel == PauseLevel.Unpaused && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
