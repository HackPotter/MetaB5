using UnityEngine;

[Trigger(Description = "Crossfades an animation clip.", DisplayPath = "Animation")]
[AddComponentMenu("Metablast/Triggers/Actions/Animations/Crossfade Animation")]
public class CrossfadeAnimation : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The name of the AnimationClip to crossfade to.")]
    private string _clipToPlay;

    [SerializeField]
    [ExpressionField(typeof(Animation), "Animation Component")]
    private Expression _animationComponent;

    [SerializeField]
    [Infobox("Whether to stop all other animations or only the ones in the layer.")]
    private PlayMode _animationPlayMode;

    [SerializeField]
    [Infobox("If true, the animation will always be played from the start. The start of the animation is defined by the Starting Point.")]
    private bool _playFromStart;

    [SerializeField]
    [Infobox("If Play From Start is enabled, this defines where the animation will be started at.")]
    private AnimationStartingPoint _startingPoint;

    [SerializeField]
    [Infobox("If Starting Point is \"Custom\", then this is the time at which the animation will be started.")]
    private float _startingTimeForCustomStartingPoint;

    [SerializeField]
    [Infobox("How quickly the animation will be played. A value of 1 indicates it will be played at the default speed.")]
    private float _playbackSpeed = 1;

    [SerializeField]
    [Infobox("Defines how the animation will wrap if it reaches the end.")]
    private WrapMode _wrapMode;

    [SerializeField]
    [Infobox("Defines how the animation will be blended with any other animations currently being played.")]
    private AnimationBlendMode _blendMode;

    [SerializeField]
    [Infobox("The layer in which the animation will be played.")]
    private int _animationLayer;

    [SerializeField]
    [Infobox("The amount of time before the animation has been blended completely to the AnimationClip being played.")]
    private float _fadeLength = 0.3f;

    [SerializeField]
    [Infobox("The weight of the animation within its layer.")]
    private float _weight = 1.0f;
#pragma warning restore 0067, 0649

    protected override void Start()
    {
        if (!_animationComponent)
        {
            DebugFormatter.LogError(this, "Animation component cannot be null.");
            this.enabled = false;
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
        if (!animation[_clipToPlay])
        {
            DebugFormatter.LogError(this, "Animation component does not contain a clip called {0}", _clipToPlay);
            return;
        }

        AnimationState state = animation[_clipToPlay];
        if (_playFromStart)
        {
            switch (_startingPoint)
            {
                case AnimationStartingPoint.Beginning:
                    animation.Rewind(_clipToPlay);
                    break;
                case AnimationStartingPoint.End:
                    state.time = animation[_clipToPlay].length;
                    break;
                case AnimationStartingPoint.Custom:
                    state.time = _startingTimeForCustomStartingPoint;
                    break;
            }
        }

        state.layer = _animationLayer;
        state.speed = _playbackSpeed;
        state.blendMode = _blendMode;
        state.wrapMode = _wrapMode;
        state.weight = _weight;

        animation.CrossFade(_clipToPlay, _fadeLength, _animationPlayMode);
    }
}

