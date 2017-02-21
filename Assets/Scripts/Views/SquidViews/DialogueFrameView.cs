using System.Collections;
using Squid;
using UnityEngine;

public class DialogueFrameView : Frame, IDialogueView
{
    public string MessageFrameTitle
    {
        get
        {
            return _titleLabel.Text;
        }
        set
        {
            _titleLabel.Text = value;
        }
    }

    public string MessageText
    {
        get
        {
            return _unformattedDisplayText;
        }
        set
        {
            _unformattedDisplayText = value;
        }
    }

    public event DialogueBranchTaken OnDialogueBranchTaken;
    public event DialogueViewProgressed OnDialogueViewProgressed;


    private bool _allowProgression = false;
    private int _lastDisplayCharacterIndex;
    private string _unformattedDisplayText;
    private Control _dialogueFrame;
    private DialogueNodeComponent _currentNode;
    private FlowLayoutFrame _actionButtonsFrame;
    private Label _titleLabel;
    private TextArea _messageTextArea;

    public void Show(DialogueNodeComponent dialogueNode)
    {
		MetablastUI.Instance.BiologView.SetTint(0.5f);
        _currentNode = dialogueNode;
        if (_currentNode == null)
        {
            _dialogueFrame.Visible = false;
			MetablastUI.Instance.BiologView.SetTint(0f);
            return;
        }
        else
        {
            _dialogueFrame.Visible = true;
        }
        _currentNode = dialogueNode;

        MessageFrameTitle = dialogueNode.DialogueData.Sender;
        MessageText = dialogueNode.DialogueData.Message;
        _lastDisplayCharacterIndex = 0;
        SetActionButtons(dialogueNode);

        _timeAtLastChar = Time.time;
    }

    public void Hide()
    {
        Show(null);
		MetablastUI.Instance.BiologView.SetTint(0f);
    }

    public void SetActionButtons(DialogueNodeComponent dialogueNode)
    {
        foreach (Control actionButton in _actionButtonsFrame.Controls)
        {
            actionButton.MouseClick -= button_MouseClick;
        }
        _actionButtonsFrame.Controls.Clear();

        if (dialogueNode.HasBranch)
        {
            foreach (DialogueTransitionNodeComponent transitionNode in dialogueNode.DialogueTransitions)
            {
                Button button = new Button();
                button.AutoSize = AutoSize.Horizontal;
                button.Dock = DockStyle.Left;
                button.Margin = new Margin(4, 0, 0, 0);
                button.Text = transitionNode.DialogueTransitionData.TransitionText;
                button.Style = "Button - Pointed";
                button.Tag = transitionNode;
                button.MouseClick += button_MouseClick;
                _actionButtonsFrame.Controls.Add(button);
            }
        }
        else
        {
            Button button = new Button();
            button.AutoSize = AutoSize.Horizontal;
            button.Dock = DockStyle.Left;
            button.Margin = new Margin(4, 0, 0, 0);
            button.Text = "Continue...";
            button.Style = "Button - Pointed";
            button.MouseClick += continueButton_MouseClick;
            _actionButtonsFrame.Controls.Add(button);
        }
        _actionButtonsFrame.ForceFlowLayout();
    }

    public DialogueFrameView(Control dialogueFrame)
    {
        dialogueFrame.Dock = DockStyle.Fill;
        this.Dock = DockStyle.Fill;
        this.Controls.Add(dialogueFrame);
        _dialogueFrame = dialogueFrame;
        _messageTextArea = (TextArea)dialogueFrame.GetControl("Content").GetControl("Message Body");
        _titleLabel = (Label)dialogueFrame.GetControl("Top").GetControl("New Message");

        _actionButtonsFrame = (FlowLayoutFrame)dialogueFrame.GetControl("Content").GetControl("Action Buttons");
        _actionButtonsFrame.Controls.Clear();
        
        Control imageFrame = dialogueFrame.GetControl("Clara");

        _dialogueFrame.Visible = false;

        _messageTextArea.Animation.Custom(TextAnimation());
    }

    private float _timeAtLastChar;
    private IEnumerator TextAnimation()
    {
        while (true)
        {
            int charactersPerSecond = 75;
            int increment = (int)(charactersPerSecond * (Time.time - _timeAtLastChar));

            for (int i = 0; i < increment; i++)
            {
                if (_unformattedDisplayText.Length >= _lastDisplayCharacterIndex)
                {
                    _allowProgression = false;
                    _actionButtonsFrame.Visible = false;
                    _messageTextArea.Text = _unformattedDisplayText.Substring(0, _lastDisplayCharacterIndex);
                    _lastDisplayCharacterIndex++;
                    _timeAtLastChar = Time.time;
                }
                else
                {
                    _allowProgression = true;
                    _actionButtonsFrame.Visible = true;
                    break;
                }
            }
            yield return null;
        }
    }

    void button_MouseClick(Control sender, MouseEventArgs args)
    {
        if (_allowProgression)
        {
            DialogueTransitionNodeComponent transition = sender.Tag as DialogueTransitionNodeComponent;
            Button buttonSender = sender as Button;
            if (OnDialogueBranchTaken != null)
            {
                OnDialogueBranchTaken(_currentNode, transition);
            }
        }
    }

    void continueButton_MouseClick(Control sender, MouseEventArgs args)
    {
        if (_allowProgression && OnDialogueViewProgressed != null)
        {
            OnDialogueViewProgressed(_currentNode);
        }
    }
}

