using UnityEngine;

[Trigger(Description = "DEPRECATED: Use cross fade instead.", DisplayPath = "Deprecated")]
[AddComponentMenu("Metablast/Triggers/Actions/Animations/Crossfade Animation")]
public class DeprecatedCrossfadeAnimation : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _clipToPlay;

    [SerializeField]
    private Animation _animationComponent;

    [SerializeField]
    private PlayMode _animationPlayMode;

    [SerializeField]
    private bool _playFromStart;

    [SerializeField]
    private AnimationStartingPoint _startingPoint;

    [SerializeField]
    private float _startingTimeForCustomStartingPoint;

    [SerializeField]
    private float _playbackSpeed = 1;

    [SerializeField]
    private WrapMode _wrapMode;

    [SerializeField]
    private AnimationBlendMode _blendMode;

    [SerializeField]
    private int _animationLayer;

    [SerializeField]
    private float _fadeLength = 0.3f;

    [SerializeField]
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
        if (!_animationComponent[_clipToPlay])
        {
            DebugFormatter.LogError(this, "Animation component does not contain a clip called {0}", _clipToPlay);
            this.enabled = false;
            return;
        }

        AnimationState state = _animationComponent[_clipToPlay];
        if (_playFromStart)
        {
            switch (_startingPoint)
            {
                case AnimationStartingPoint.Beginning:
                    _animationComponent.Rewind(_clipToPlay);
                    break;
                case AnimationStartingPoint.End:
                    state.time = _animationComponent[_clipToPlay].length;
                    break;
                case AnimationStartingPoint.Custom:
                    state.time = _startingTimeForCustomStartingPoint;
                    break;
            }
        }
    }

    public override void OnEvent(ExecutionContext context)
    {
        AnimationState state = _animationComponent[_clipToPlay];
        if (_playFromStart)
        {
            switch (_startingPoint)
            {
                case AnimationStartingPoint.Beginning:
                    _animationComponent.Rewind(_clipToPlay);
                    break;
                case AnimationStartingPoint.End:
                    state.time = _animationComponent[_clipToPlay].length;
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

        _animationComponent.CrossFade(_clipToPlay, _fadeLength, _animationPlayMode);
    }
}

