using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Trigger(Description="Invoked when the player has grabbed an object with the tractor/impulse beam.", DisplayPath="Tools")]
public class PlayerGrabbedObject : EventSender
{
    private GameObject _grabbedGameObject;

    protected override void OnStart()
    {
        base.OnStart();
        GameContext.Instance.Player.ImpulseBeamTool.PlayerGrabbedObject += ImpulseBeamTool_PlayerGrabbedObject;
    }

    void OnDestroy()
    {
        GameContext.Instance.Player.ImpulseBeamTool.PlayerGrabbedObject -= ImpulseBeamTool_PlayerGrabbedObject;
    }

    void ImpulseBeamTool_PlayerGrabbedObject(GrabbableObject grabbedObject)
    {
        _grabbedGameObject = grabbedObject.gameObject;
        TriggerEvent();
        _grabbedGameObject = null;
    }

    protected override void PopulateContext(ExecutionContext context)
    {
        context.AddLocal("Grabbed Object", _grabbedGameObject);
    }

    private List<OutputParameterDeclaration> _declarations = new List<OutputParameterDeclaration>()
    {
        new OutputParameterDeclaration()
        {
            Name = "Grabbed Object",
            Description = "The object that has been grabbed by the player.",
            Type = typeof(GameObject)
        },
    };

    public override List<OutputParameterDeclaration> GetOutputParameterDeclarations()
    {
        return _declarations;
    }
}

