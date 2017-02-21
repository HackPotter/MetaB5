using UnityEngine;

[Trigger(Description = "Stops an animation.", DisplayPath = "Animation")]
[AddComponentMenu("Metablast/Triggers/Actions/Animations/Stop Animation")]
public class StopAnimation : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(Animation), "Animation Component")]
    private Expression _animationComponent;

    [SerializeField]
    [Infobox("If true, a specific clip will be stopped. Otherwise, all clips will be stopped.")]
    private bool _stopSpecificClip;

    [SerializeField]
    [Infobox("If Pause Specific Clip is specified, this is the name of the clip to be stopped.")]
    private string _optionalClipToStop;
#pragma warning restore 0067, 0649

    protected override void Awake()
    {
        base.Awake();

        if (_animationComponent == null)
        {
            DebugFormatter.LogError(this, "Animation component cannot be null.");
            Destroy(this);
            return;
        }
    }

    public override void OnEvent(ExecutionContext context)
    {
        Animation animation = context.Evaluate<Animation>(_animationComponent);
        if (!animation)
        {
            DebugFormatter.LogError(this, "Expected Animation component, but Expression evaluated to null.");
            return;
        }

        if (_stopSpecificClip)
        {
            if (!animation[_optionalClipToStop])
            {
                DebugFormatter.LogError(this, "Animation component does not contain a clip called {0}", _optionalClipToStop);
                return;
            }
            animation.Stop(_optionalClipToStop);
        }
        else
        {
            animation.Stop();
        }
    }
}

