#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using UnityEngine.UI;

namespace Metablast.UI
{
    [RequireComponent(typeof(TextAnimator))]
    public class DialogueView : MonoBehaviour, IDialogueView
    {
        public event DialogueBranchTaken OnDialogueBranchTaken;
        public event DialogueViewProgressed OnDialogueViewProgressed;

        private TextAnimator _textAnimator;
        private bool _allowProgression = false;
        private DialogueNodeComponent _currentNode;

#pragma warning disable 0067, 0649
        [SerializeField]
        private RectTransform _buttonArea;
        [SerializeField]
        private DialogueButton _buttonPrefab;

        [SerializeField]
        private Text _titleText;
        [SerializeField]
        private Text _messageText;
#pragma warning restore 0067, 0649

        public string MessageFrameTitle
        {
            get { return _titleText.text; }
            set { _titleText.text = value; }
        }

        public string MessageText
        {
            get { return _messageText.text; }
            set { _messageText.text = value; }
        }

        void Awake()
        {
            _textAnimator = GetComponent<TextAnimator>();
        }
        
        public void Show(DialogueNodeComponent dialogueNode)
        {
            ClearButtons();
            _currentNode = dialogueNode;
            if (_currentNode == null)
            {
                this.gameObject.SetActive(false);
                MetablastUI.Instance.BiologView.SetTint(0f);
                return;
            }
            else
            {
                gameObject.SetActive(true);
            }

            MessageFrameTitle = dialogueNode.DialogueData.Sender;
            _allowProgression = false;
            _textAnimator.ShowText(dialogueNode.DialogueData.Message, (t) => MessageText = t,
                () =>
                {
                    //Debug.Log("Setting buttons");
                    SetActionButtons(dialogueNode);
                    _allowProgression = true;
                });
        }

        public void Hide()
        {
        }

        private void SetActionButtons(DialogueNodeComponent dialogueNode)
        {
            if (dialogueNode.HasBranch)
            {
                foreach (DialogueTransitionNodeComponent transitionNode in dialogueNode.DialogueTransitions)
                {
                    // Create a button.
                    DialogueButton button = GameObject.Instantiate(_buttonPrefab) as DialogueButton;

                    button.ButtonText = transitionNode.DialogueTransitionData.TransitionText;

                    button.transform.SetParent(_buttonArea);
                    button.transform.localScale = Vector3.one;
                    button.IsTransition = true;
                    button.TransitionNode = transitionNode;
                    button.ButtonPressed.AddListener(ButtonPressed);
                    button.gameObject.SetActive(true);
                }
            }
            else
            {
                // Just do one button.
                DialogueButton button = GameObject.Instantiate(_buttonPrefab) as DialogueButton;
                button.ButtonText = "Continue...";
                button.transform.SetParent(_buttonArea);
                button.transform.localScale = Vector3.one;
                button.ButtonPressed.AddListener(ButtonPressed);
                button.gameObject.SetActive(true);
            }
        }

        private void ClearButtons()
        {
            MessageText = "";
            MessageFrameTitle = "";
            for (int i = 0; i < _buttonArea.childCount; i++)
            {
                Destroy(_buttonArea.GetChild(i).gameObject);
            }
        }

        private void ButtonPressed(DialogueButton button)
        {
            if (button.IsTransition)
            {
                if (OnDialogueBranchTaken != null)
                {
                    OnDialogueBranchTaken(_currentNode, button.TransitionNode);
                }
            }
            else
            {
                if (OnDialogueViewProgressed != null) OnDialogueViewProgressed(_currentNode);

            }
        }
    }
}
