using System.Collections.Generic;
using UnityEngine;

[Trigger(Description = "Attempts to get an Animation component from the given GameObject. If no Animation component is found, then the actions under the filter are invoked.", DisplayPath = "Animation")]
public class GetAnimationComponentFromGameObject : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(GameObject), "GameObject to get Animation component from")]
    private Expression _gameObject;
#pragma warning restore 0067, 0649

    private Animation _animationComponent;

    public override void OnEvent(ExecutionContext context)
    {
        GameObject gameObject = context.Evaluate<GameObject>(_gameObject);
        if (!gameObject)
        {
            DebugFormatter.LogError(this, "Expecting GameObject, but Expression evaluated to null.");
            return;
        }

        _animationComponent = context.Evaluate<GameObject>(_gameObject).GetComponent<Animation>();
        if (!_animationComponent)
        {
            DebugFormatter.LogError(this, "Could not retrieve Animation component from GameObject {0}", gameObject.name);
            return;
        }

        TriggerEvent(context);

        _animationComponent = null;
    }

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Animation Component", _animationComponent);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name="Animation Component",
            Description = "The animation component found on the game object.",
            Type = typeof(Animation)
        }
    };
}

