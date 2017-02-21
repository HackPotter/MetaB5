using UnityEngine;

[Trigger(Description = "Invoked when the specified animation reaches the AnimationKeyframeFunction with the given tag.", DisplayPath = "Animation")]
[AddComponentMenu("Metablast/Triggers/Events/Animations/Animation Keyframe Event")]
public class CameraLookingAtObject : EventSender
{

#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("This event will be invoked when this camera looks at the given object.")]
    private Camera _camera;

    [SerializeField]
    [Infobox("The object for which this event will be invoked when the camera is looking at it.")]
    private Transform _objectToLookAt;

    [SerializeField]
    [Range(0, 1)]
    [Infobox("How perfectly the camera has to be facing the object for this to be invoked. At 0, the camera will have to be perfectly facing it. At 1, the camera will have to be facing within 90 degrees of it.")]
    private float _tolerance;
#pragma warning restore 0067, 0649

    
    void Update()
    {
        if (_camera)
        {
            Vector3 cameraForward = _camera.transform.forward;
            Vector3 cameraToObject = (_objectToLookAt.position - _camera.transform.position).normalized;
            

            // Range -1 to 1
            float dot = Vector3.Dot(cameraForward, cameraToObject);
            
            // If tolerance is 1, 
            if (dot - (1 - _tolerance) >= 0)
            {
                TriggerEvent();
            }
        }
    }
}

