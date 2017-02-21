using UnityEngine;

// Third person. Mode change.
// Aim with mouse. Click to start charging. Release to shine light.

public class AxisAlignedBillboard : MonoBehaviour
{
    public Camera Camera;

    void Start()
    {
        if (!Camera)
        {
            DebugFormatter.LogError(this, "Camera must not be null.");
            this.enabled = false;
            return;
        }
    }

    void Update()
    {
        Vector3 toCamera = (Camera.transform.position - transform.position).normalized;
        transform.LookAt(transform.position + transform.forward, toCamera);
    }
}

