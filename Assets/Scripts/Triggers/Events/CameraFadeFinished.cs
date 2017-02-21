using UnityEngine;


[Trigger(Description = "Invoked when the camera fade has completed.\nTo perform a camera fade, use the FadeCamera action.", DisplayPath = "Camera")]
[AddComponentMenu("Metablast/Triggers/Events/Camera/Camera Fade Finished")]
public class CameraFadeFinished : EventSender
{
    private CameraFade _cameraFade;

    protected override void OnAwake()
    {
        _cameraFade = CameraFade.Instance;
        _cameraFade.OnCameraFadeStabilized += new CameraFadeStabilized(_cameraFade_OnCameraFadeStabilized);
    }

    void _cameraFade_OnCameraFadeStabilized()
    {
        TriggerEvent();
    }
}

