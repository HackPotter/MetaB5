using System.Collections.Generic;
using UnityEngine;

[Trigger(Description = "Invoked when the player has dropped an object he was holding with the tractor/impulse beam.", DisplayPath = "Tools")]
public class PlayerDroppedObject : EventSender
{
    private GameObject _grabbedGameObject;

    protected override void OnStart()
    {
        Debug.Log("Registering to drop event - " + gameObject.name);
        base.OnStart();
        GameContext.Instance.Player.ImpulseBeamTool.PlayerDroppedObject += ImpulseBeamTool_PlayerGrabbedObject;
    }

    void OnDestroy()
    {
        GameContext.Instance.Player.ImpulseBeamTool.PlayerDroppedObject -= ImpulseBeamTool_PlayerGrabbedObject;
    }

    void ImpulseBeamTool_PlayerGrabbedObject(GrabbableObject grabbedObject)
    {
        Debug.Log("An object has been dropped - " + gameObject.name);
        _grabbedGameObject = grabbedObject.gameObject;
        TriggerEvent();
        _grabbedGameObject = null;
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Dropped Object", _grabbedGameObject);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name = "Dropped Object",
            Description = "The object that has been dropped by the player",
            Type = typeof(GameObject)
        },
    };

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }
}

