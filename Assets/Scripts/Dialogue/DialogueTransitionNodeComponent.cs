using UnityEngine;

public class DialogueTransitionNodeComponent : MonoBehaviour
{
    private DialogueNodeComponent _successorNode;

    [SerializeField]
    private DialogueTransitionNodeData _dialogueTransitionData;

    public DialogueTransitionNodeData DialogueTransitionData
    {
        get { return _dialogueTransitionData; }
        set { _dialogueTransitionData = value; }
    }

    public DialogueNodeComponent DialogueNodeComponent
    {
        get { return _successorNode; }
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            DialogueNodeComponent dialogue = child.GetComponent<DialogueNodeComponent>();

            if (dialogue != null)
            {
                if (_successorNode == null)
                {
                    _successorNode = dialogue;
                }
                else
                {
                    Debug.Log("No??");
                    DebugFormatter.LogError(this, "Found multiple dialogue node children. A {0} component can only have one {1} child.", GetType().Name, typeof(DialogueNodeComponent).Name);
                    return;
                }
            }
        }
    }
}

