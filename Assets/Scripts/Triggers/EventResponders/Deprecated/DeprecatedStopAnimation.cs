using UnityEngine;

[Trigger(Description = "DEPRECATED: Use StopAnimation instead.", DisplayPath = "Deprecated")]
[AddComponentMenu("Metablast/Triggers/Actions/Animations/Stop Animation")]
public class DeprecatedStopAnimation : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private bool _stopSpecificClip;

    [SerializeField]
    private string _optionalClipToStop;
#pragma warning restore 0067, 0649

    protected override void Awake()
    {
        base.Awake();

        if (_animation == null)
        {
            DebugFormatter.LogError(this, "Animation cannot be null.");
            Destroy(this);
            return;
        }
    }

    public override void OnEvent(ExecutionContext context)
    {
        if (_stopSpecificClip)
        {
            _animation.Stop(_optionalClipToStop);
        }
        else
        {
            _animation.Stop();
        }
    }
}

