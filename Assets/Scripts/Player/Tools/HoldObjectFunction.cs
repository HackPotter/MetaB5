using UnityEngine;

public class HoldObjectFunction : MonoBehaviour, IFunction
{
    private ControlServices _controlServices;
    private CameraControllerInterface _cameraController;

    public GrabbableObject _grabbedObject;
    private Vector3 _targetOffset;
    private float _holdingForce;

    void Awake()
    {
        _controlServices = GetComponent<ControlServices>();
        _cameraController = _controlServices.ControlledCamera;
    }

    void Update()
    {
        if (_grabbedObject != null)
        {
            Vector3 offset = enabled ? _cameraController.Camera.transform.forward * 3 : transform.forward * 3;
            Vector3 targetPosition = transform.TransformPoint(_targetOffset) + offset;
            Vector3 currentPosition = _grabbedObject.GetComponent<Rigidbody>().worldCenterOfMass;

            Vector3 positionError = targetPosition - currentPosition;

            Vector3 force = positionError * _holdingForce * Time.fixedDeltaTime;

            _grabbedObject.GetComponent<Rigidbody>().AddForceAtPosition(force, currentPosition, ForceMode.Impulse);
        }
    }

    public void HoldObject(GrabbableObject grabbableObject, Vector3 targetOffset, float holdingForce)
    {
        _grabbedObject = grabbableObject;
        _targetOffset = targetOffset;
        _holdingForce = holdingForce;
    }

    public void ReleaseObject()
    {
        _grabbedObject = null;
    }

    public void OnAcquiredControl()
    {
    }

    public void OnLostControl()
    {
    }
}

