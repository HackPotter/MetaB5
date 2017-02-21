using UnityEngine;
using System.Collections.Generic;

[Trigger(Description="Invoked when the specified animation reaches the AnimationKeyframeFunction with the given tag.",DisplayPath="Animation")]
[AddComponentMenu("Metablast/Triggers/Events/Animations/Animation Keyframe Event")]
public class AnimationKeyframeEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("An AnimationKeyframeFunction that will be invoked.")]
    private AnimationKeyframeFunction _animationKeyframe;

    [SerializeField]
    [Infobox("The tag of provided of the AnimationKeyframeFunction that will be invoked.")]
    private string _animationTag;
#pragma warning restore 0067, 0649

    private GameObject _animatedGameObject;

    protected override void OnStart()
    {
        if (_animationKeyframe == null)
        {
            DebugFormatter.LogError(this, "AnimationKeyframeFunction cannot be null.");
            Destroy(this);
            return;
        }

        _animationKeyframe.OnAnimationEventTriggered += new AnimationChannelTriggered(_animationKeyframe_OnAnimationEventTriggered);
    }

    void _animationKeyframe_OnAnimationEventTriggered(string tag, GameObject sender)
    {
        if (tag == _animationTag)
        {
            _animatedGameObject = sender;
            TriggerEvent();
            _animatedGameObject = null;
        }
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("AnimationKeyframeEvent: AnimatedGameObject", _animatedGameObject);
    }

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _outputParameterDeclarations;
    }

    private List<OutputParameterDeclaration> _outputParameterDeclarations = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name="AnimationKeyframeEvent: AnimatedGameObject",
            Type = typeof(GameObject)
        }
    };
}

