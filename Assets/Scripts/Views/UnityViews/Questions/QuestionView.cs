#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Metablast.UI
{
    [Serializable]
    public class QuestionAnsweredCorrectly : UnityEvent { }
    
    [Serializable]
    public class QuestionViewShow : UnityEvent { }
    
    public class QuestionView : MonoBehaviour, IQuestionView
    {
        public Image QuestionImage;
        public Text QuestionText;
        public QuestionAnswerButton QuestionAnswerPrefab;
        public GridLayoutGroup QuestionAnswerGrid;
        public Button ExitButton;

        public AnimationClip ShowAnimation;
        public AnimationClip HideAnimation;
        
        public QuestionAnsweredCorrectly QuestionAnsweredCorrectly;
        public QuestionViewShow QuestionViewShow;
        public QuestionViewShow QuestionViewHide;

        public event Action QuestionViewExited;

        private QuestionProgress _currentQuestionProgress;
        private QuestionData _currentQuestion;

        void Awake()
        {
            ExitButton.onClick.AddListener(ExitButtonPressed);
        }

        private void ExitButtonPressed()
        {
            if (HideAnimation && GetComponent<Animation>())
            {
                GetComponent<Animation>().Play(HideAnimation.name);
            }

            QuestionViewHide.Invoke();
            if (QuestionViewExited != null)
                QuestionViewExited();

            //GameState.Instance.PauseLevel = PauseLevel.Unpaused;
        }

        public void Show(QuestionData question)
        {
            QuestionViewShow.Invoke();
            for (int i = 0; i < QuestionAnswerGrid.transform.childCount; i++)
            {
                GameObject.Destroy(QuestionAnswerGrid.transform.GetChild(i).gameObject);
            }
            //GameState.Instance.PauseLevel = PauseLevel.Cutscene;
            QuestionProgress progress = GameContext.Instance.Player.QuestionProgress.GetQuestionProgress(question);

            if (ShowAnimation && GetComponent<Animation>())
            {
                GetComponent<Animation>().Play(ShowAnimation.name);
            }
            _currentQuestion = question;
            _currentQuestionProgress = progress;

            QuestionText.text = question.QuestionText;

            foreach (var answer in question.QuestionAnswers)
            {
                CreateButton(answer, progress).QuestionAnswered.AddListener(QuestionAnswered);
            }
        }

        private void QuestionAnswered(QuestionAnswer answer)
        {
            if (!_currentQuestionProgress.Completed)
            {
                _currentQuestionProgress.ChooseAnswer(answer);
            }
        }

        private QuestionAnswerButton CreateButton(QuestionAnswer answer, QuestionProgress progress)
        {
            QuestionAnswerButton button = GameObject.Instantiate(QuestionAnswerPrefab) as QuestionAnswerButton;

            if (!button)
            {
                Debug.LogError("Error: QuestionView doesn't have its QuestionAnswerButton prefab set.", this);
                throw new Exception();
            }


            button.Answer = answer;
            button.QuestionText.text = answer.QuestionAnswerText;
            button.gameObject.SetActive(true);
            button.transform.SetParent(QuestionAnswerGrid.transform);

            if (progress.AnswerChosen(answer) || progress.Completed)
            {
                button.InitializeButtonAlreadyAnswered.Invoke();
            }
            else
            {
                button.InitializeButtonNotYetAnswered.Invoke();
            }


            return button;
        }
    }
}
