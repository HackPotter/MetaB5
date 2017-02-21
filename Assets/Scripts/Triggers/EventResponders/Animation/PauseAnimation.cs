using UnityEngine;

[Trigger(Description = "Pauses an animation", DisplayPath = "Animation")]
[AddComponentMenu("Metablast/Triggers/Actions/Animations/Pause Animation")]
public class PauseAnimation : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(Animation), "Animation Component")]
    private Expression _animationComponent;

    [SerializeField]
    [Infobox("If true, a specific clip will be paused. Otherwise, all clips will be paused.")]
    private bool _pauseSpecificClip;

    [SerializeField]
    [Infobox("If Pause Specific Clip is specified, this is the name of the clip to be paused.")]
    private string _optionalClipToPause;
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

        if (_pauseSpecificClip)
        {
            if (!animation[_optionalClipToPause])
            {
                DebugFormatter.LogError(this, "Animation component does not contain a clip called {0}", _optionalClipToPause);
                return;
            }
            animation[_optionalClipToPause].speed = 0;
        }
        else
        {
            animation[animation.clip.name].speed = 0;
        }
    }
}

