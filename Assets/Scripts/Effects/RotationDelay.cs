using UnityEngine;

public class RotationDelay : MonoBehaviour
{
    public CameraControllerInterface CameraController;

    void Update()
    {
        Vector3 direction = CameraController.transform.InverseTransformDirection(CameraController.TargetForward);
        direction.x = -direction.x;

        direction = CameraController.transform.TransformDirection(direction);


        transform.LookAt(transform.position + Quaternion.FromToRotation(Vector3.forward, Vector3.right) * direction);
    }
}

