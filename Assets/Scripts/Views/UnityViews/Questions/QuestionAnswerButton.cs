using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Metablast.UI
{
    public class QuestionAnsweredEvent : UnityEvent<QuestionAnswer> { }

    [Serializable]
    public class SetQuestionButtonState : UnityEvent { }
    public class QuestionAnswerButton : MonoBehaviour
    {
        public Text QuestionText;
        public Button AnswerButton;
 
        
        public SetQuestionButtonState InitializeButtonAlreadyAnswered;
        public SetQuestionButtonState InitializeButtonNotYetAnswered;

        public QuestionAnsweredEvent QuestionAnswered = new QuestionAnsweredEvent();

        void Awake()
        {
            AnswerButton.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            QuestionAnswered.Invoke(Answer);
        }

        public QuestionAnswer Answer
        {
            get;
            set;
        }

        public bool QuestionHasBeenAnswered
        {
            get;
            set;
        }
    }
}
