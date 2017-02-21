using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Metablast.UI
{
    [Serializable]
    public class DialogueButtonPressed : UnityEvent<DialogueButton> { }

	public class DialogueButton : MonoBehaviour
	{
        private DialogueButtonPressed buttonPressed = new DialogueButtonPressed();

        [SerializeField]
        private Button _button;
        
        [SerializeField]
        private Text _buttonText;

        void Awake()
        {
            _button.onClick.AddListener(MouseClick);
        }

        void MouseClick()
        {
            buttonPressed.Invoke(this);
        }

        public string ButtonText
        {
            get { return _buttonText.text; }
            set { _buttonText.text = value; }
        }

        public Button Button
        {
            get { return _button; }
        }

        public bool IsTransition
        {
            get;
            set;
        }

        public DialogueTransitionNodeComponent TransitionNode
        {
            get;
            set;
        }

        public DialogueButtonPressed ButtonPressed
        {
            get { return buttonPressed; }
        }
	}
}
