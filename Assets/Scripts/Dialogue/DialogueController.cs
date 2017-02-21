using Metablast.UI;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public event DialogueNodeReached OnDialogueNodeReached;
	public event DialogueNodeReached OnDialogueNodeCompleted;
    public event DialogueBranchTaken OnDialogueBranchTaken;
    
    [SerializeField]
    private DialogueView _view;
    
    void Start()
    {
        _view.OnDialogueViewProgressed += new DialogueViewProgressed(_view_OnDialogueViewProgressed);
        _view.OnDialogueBranchTaken += new DialogueBranchTaken(_view_OnDialogueBranchTaken);
    }

    void _view_OnDialogueBranchTaken(DialogueNodeComponent currentNode, DialogueTransitionNodeComponent node)
    {
		if (OnDialogueNodeCompleted != null)
		{
			OnDialogueNodeCompleted(currentNode);
		}
		
        _view.Show(node.DialogueNodeComponent);

        if (OnDialogueBranchTaken != null)
        {
            OnDialogueBranchTaken(currentNode, node);
        }
        if (OnDialogueNodeReached != null)
        {
            OnDialogueNodeReached(node.DialogueNodeComponent);
        }
    }

    void _view_OnDialogueViewProgressed(DialogueNodeComponent node)
    {
		if (OnDialogueNodeCompleted != null)
		{
			OnDialogueNodeCompleted(node);
		}
		
		if (node.NextDialogueNode == null && GameState.Instance.PauseLevel == PauseLevel.Dialogue)
        {
            GameState.Instance.PauseLevel = PauseLevel.Unpaused;
			_view.Show(node.NextDialogueNode);
			return;
        }
		
        _view.Show(node.NextDialogueNode);
        if (OnDialogueNodeReached != null)
        {
            OnDialogueNodeReached(node.NextDialogueNode);
        }
    }

    public void StartDialogue(DialogueNodeComponent dialogueRoot)
    {
        if (dialogueRoot.PausesGame && GameState.Instance.PauseLevel < PauseLevel.Dialogue)
        {
            GameState.Instance.PauseLevel = PauseLevel.Dialogue;
        }
        _view.Show(dialogueRoot);
        if (OnDialogueNodeReached != null)
        {
            OnDialogueNodeReached(dialogueRoot);
        }
    }
}
