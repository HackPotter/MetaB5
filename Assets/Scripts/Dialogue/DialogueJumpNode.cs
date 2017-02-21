using System.Collections.Generic;
using UnityEngine;

public class DialogueJumpNode : DialogueNodeComponent
{
    [SerializeField]
    private DialogueNodeComponent _jumpTarget;

    public override DialogueNodeData DialogueData
    {
        get
        {
            return _jumpTarget.DialogueData;
        }
    }

    public override List<DialogueTransitionNodeComponent> DialogueTransitions
    {
        get
        {
            return _jumpTarget.DialogueTransitions;
        }
    }

    public override bool HasBranch
    {
        get
        {
            return _jumpTarget.HasBranch;
        }
    }

    public override DialogueNodeComponent NextDialogueNode
    {
        get
        {
            return _jumpTarget.NextDialogueNode;
        }
    }

    public DialogueNodeComponent JumpTarget
    {
        get { return _jumpTarget; }
        set
        {
            if (_jumpTarget != this)
            {
                _jumpTarget = value;
            }
            else
            {
                DebugFormatter.LogError(this, "Cannot jump to self.");
            }
        }
    }

    protected override void Start()
    {
    }
}

