using System.Collections.Generic;
using UnityEngine;

[Trigger(Description = "Finds the GameObject that is the owner of the given component.")]
public class GetGameObjectFromComponent : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(Component), "Component from which to retrieve GameObject")]
    private Expression _component;
#pragma warning restore 0067, 0649

    private GameObject _gameObject;

    public override void OnEvent(ExecutionContext context)
    {
        Component component = context.Evaluate<Component>(_component);
        if (!component)
        {
            DebugFormatter.LogError(this, "Expecting Component, but Expression evaluated to null.");
            return;
        }

        _gameObject = component.gameObject;

        TriggerEvent(context);
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Game Object", _gameObject);
    }

    private static readonly List<OutputParameterDeclaration> _outputParameters = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name = "Game Object",
            Type = typeof(GameObject)
        },
    };
    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _outputParameters;
    }
}
