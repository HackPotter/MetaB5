using System;

public interface IDialogueView
{
    event DialogueBranchTaken OnDialogueBranchTaken;
    event DialogueViewProgressed OnDialogueViewProgressed;
    void Show(DialogueNodeComponent dialogueNode);
    void Hide();
}

