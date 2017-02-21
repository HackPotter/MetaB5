using System.Collections.Generic;
using UnityEngine;

[Trigger(Description = "Attempts to find a GameObject with the given name that is a child of the given game object. If found, the actions under this filter are invoked.", DisplayPath = "GameObject")]
public class GetChildGameObject : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The name of the child game object to look for.")]
    private string _gameObjectName = "";

    [SerializeField]
    [ExpressionField(typeof(GameObject), "GameObject to get child GameObject from")]
    private Expression _gameObject;
#pragma warning restore 0067, 0649

    private GameObject _childGameObject;

    public override void OnEvent(ExecutionContext context)
    {
        GameObject gameObject = context.Evaluate<GameObject>(_gameObject);
        if (!gameObject)
        {
            DebugFormatter.LogError(this, "Expecting GameObject, but Expression evaluated to null.");
            return;
        }

        Transform transform = gameObject.transform.FindChildInHierarchy(_gameObjectName);
        if (!transform)
        {
            DebugFormatter.LogError(this, "Could not retrieve Animation component from GameObject {0}", gameObject.name);
            return;
        }

        _childGameObject = transform.gameObject;
        TriggerEvent(context);
        _childGameObject = null;
    }

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Child GameObject", _childGameObject);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name="Child GameObject",
            Description = "The child game object with the given name.",
            Type = typeof(GameObject)
        }
    };
}

