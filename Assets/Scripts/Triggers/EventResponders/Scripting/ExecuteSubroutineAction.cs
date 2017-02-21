using System;
using UnityEngine;

[Trigger(DisplayPath = "Triggers")]
[AddComponentMenu("Metablast/Triggers/Actions/Execute Subroutine")]
public class ExecuteSubroutineAction : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private SubroutineEvent _subroutine;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        _subroutine.InvokeSubroutine();
    }
}

