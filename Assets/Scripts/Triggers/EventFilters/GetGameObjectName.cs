using System.Collections.Generic;
using UnityEngine;

[Trigger(Description = "Gets the name of the given GameObject.", DisplayPath = "GameObject")]
public class GetGameObjectName : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(GameObject), "GameObject to get Name from")]
    private Expression _gameObject;
#pragma warning restore 0067, 0649

    private string _name;

    public override void OnEvent(ExecutionContext context)
    {
        GameObject gameObject = context.Evaluate<GameObject>(_gameObject);
        if (!gameObject)
        {
            DebugFormatter.LogError(this, "Expecting GameObject, but Expression evaluated to null.");
            return;
        }

        _name = gameObject.name;
        TriggerEvent(context);
        _name = null;
    }

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("GameObject Name", _name);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name="GameObject Name",
            Type = typeof(string)
        }
    };
}

