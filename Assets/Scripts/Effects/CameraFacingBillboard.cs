using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;

    void Start()
    {
        if (!m_Camera)
        {
            DebugFormatter.LogError(this, "Camera cannot be null.");
            this.enabled = false;
            return;
        }
    }

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back,
            m_Camera.transform.rotation * Vector3.up);
    }
}