using UnityEngine;

[Trigger(Description = "Fades the camera to the given color.", DisplayPath = "UI")]
[AddComponentMenu("Metablast/Triggers/Actions/Camera/Fade Camera")]
public class FadeCamera : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("How long it will take to fade the camera completely to the given color.")]
    private float _duration;

    [SerializeField]
    [Infobox("The color the screen will be when the camera has finished fading.")]
    private Color _fadeColor;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        CameraFade.Instance.StartFade(_fadeColor, _duration);
    }
}
