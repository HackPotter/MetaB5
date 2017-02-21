using System.Collections.Generic;
using UnityEngine;

public delegate void DialogueNodeReached(DialogueNodeComponent node);
public delegate void DialogueBranchTaken(DialogueNodeComponent currentNode, DialogueTransitionNodeComponent node);
public delegate void DialogueViewProgressed(DialogueNodeComponent node);

public class DialogueNodeComponent : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    public bool _pauseGame;

    [SerializeField]
    public DialogueNodeData _dialogueData;
#pragma warning restore 0067, 0649

    private List<DialogueTransitionNodeComponent> _childrenNodes = new List<DialogueTransitionNodeComponent>();
    private DialogueNodeComponent _successor;

    public bool PausesGame
    {
        get { return _pauseGame; }
        set { _pauseGame = value; }
    }

    public virtual DialogueNodeData DialogueData
    {
        get { return _dialogueData; }
        set { _dialogueData = value; }
    }

    public virtual bool HasBranch
    {
        get { return _childrenNodes.Count > 0; }
    }

    public virtual List<DialogueTransitionNodeComponent> DialogueTransitions
    {
        get { return _childrenNodes; }
    }

    public virtual DialogueNodeComponent NextDialogueNode
    {
        get { return _successor; }
    }

    protected virtual void Start()
    {
        foreach (Transform child in transform)
        {
            _childrenNodes.AddRange(child.GetComponents<DialogueTransitionNodeComponent>());

            DialogueNodeComponent successor = child.GetComponent<DialogueNodeComponent>();
            if (successor)
            {
                _successor = successor;
            }
        }

        if (HasBranch && _successor)
        {
            DebugFormatter.LogError(this, "Dialogue node has both branch transitions and immediate successor. Dialogue nodes can contain one or the other, but not both.");
        }
    }
}

