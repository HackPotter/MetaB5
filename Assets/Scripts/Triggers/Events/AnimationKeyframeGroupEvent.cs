using System.Collections.Generic;
using UnityEngine;

[Trigger(Description = "Invoked when a member the specified animation function group reaches the AnimationKeyframeFunction in its animation with the given tag.", DisplayPath = "Animation")]
[AddComponentMenu("Metablast/Triggers/Events/Animations/Animation Keyframe Group Event")]
public class AnimationKeyframeGroupEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The name of the animation keyframe group.")]
    private string _animationGroup;

    [SerializeField]
    [Infobox("The tag of the function that will be played.")]
    private string _animationTag;
#pragma warning restore 0067, 0649

    private GameObject _animatedGameObject;

    protected override void OnStart()
    {
        if (string.IsNullOrEmpty(_animationGroup))
        {
            DebugFormatter.LogError(this, "Animation group cannot be an empty string.");
            enabled = false;
            return;
        }
        if (string.IsNullOrEmpty(_animationTag))
        {
            DebugFormatter.LogError(this, "Animation tag cannot be an empty string.");
            enabled = false;
            return;
        }
    }

    void OnEnable()
    {
        AnimationKeyframeFunction.RegisterGroupEventHandler(_animationGroup, HandleGroupMemberAnimationTriggered);
    }

    void OnDisable()
    {
        AnimationKeyframeFunction.DeregisterGroupEventHandler(_animationGroup, HandleGroupMemberAnimationTriggered);
    }

    void HandleGroupMemberAnimationTriggered(string tag, string group, GameObject sender)
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
        context.AddLocal("AnimationGroupKeyframeEvent: AnimatedGameObject", _animatedGameObject);
    }

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _outputParameterDeclarations;
    }

    private List<OutputParameterDeclaration> _outputParameterDeclarations = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name="AnimationGroupKeyframeEvent: AnimatedGameObject",
            Type = typeof(GameObject)
        }
    };
}

