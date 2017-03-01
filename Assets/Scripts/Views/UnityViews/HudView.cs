using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Metablast.UI
{
    public class HudView : UIScreen, IHudView
    {
#pragma warning disable 0067, 0649
        [SerializeField]
        private AnimationClip _showAnimation;
        [SerializeField]
        private Animation _animation;
        [SerializeField]
        private RectTransform _objectivePanel;
        [SerializeField]
        private ObjectiveListItem _objectiveListItemPrefab;
        [SerializeField]
        private Button _biologButton;
        [SerializeField]
        private Button _optionsButton;
        [SerializeField]
        private Button _resumeGameButton;
        [SerializeField]
        private Button _exitGameButton;
        [SerializeField]
        private Text _scoreText;
#pragma warning restore 0067, 0649

        private bool _isShowing;

        public ContextMessageView _contextMessageView;
        public TransmissionView _transmissionView;

        public event Action BiologButtonPressed;
        public event Action OptionsButtonPressed;
        public event Action ResumeGameButtonPressed;
        public event Action ExitGameButtonPressed;

        public IContextMessageView ContextMessageView
        {
            get { return _contextMessageView; }
        }

        public ITransmissionView TransmissionView
        {
            get { return _transmissionView; }
        }

        public IObjectiveView ObjectiveFrameView
        {
            get { return null; }
        }

        void Start()
        {
            _showAnimation.SampleAnimation(gameObject, 0);

            _biologButton.onClick.AddListener(BiologButtonPressedHandler);
            _resumeGameButton.onClick.AddListener(ResumeGameButtonPressedHandler);
            _optionsButton.onClick.AddListener(OptionsButtonPressedHandler);
            _exitGameButton.onClick.AddListener(ExitGameButtonPressedHandler);

            GameContext.Instance.Player.CurrentObjectives.UserObjectiveAdded += CurrentObjectives_UserObjectiveAdded;
        }


        void Update()
        {
            // TODO make an event to notify...
            _scoreText.text = GameContext.Instance.Player.Points.ToString();
        }


        public override void Show(Action onComplete, float animationTime = 0.4f)
        {

            Debug.Log("Showing HUD");
            _animation.clip = _showAnimation;
            if (!_animation.isPlaying)
            {
                _animation[_showAnimation.name].time = _showAnimation.length;
            }

            _animation[_showAnimation.name].speed = -1f;
            StartCoroutine(PlayAnimationCoroutine(_showAnimation, onComplete));
            _isShowing = true;
        }

        public override void Hide(Action onComplete, float animationTime = 0.4f)
        {
            _animation.clip = _showAnimation;
            _animation[_showAnimation.name].speed = 1f;
            StartCoroutine(PlayAnimationCoroutine(_showAnimation, onComplete));
            _isShowing = false;
        }

        private IEnumerator PlayAnimationCoroutine(AnimationClip animationClip, Action onComplete)
        {
            _animation.Play(animationClip.name, PlayMode.StopAll);
            while (_animation.isPlaying)
            {
                yield return null;
            }

            Debug.Log("Animation Finished!");
            if (onComplete != null)
            {
                Debug.Log("Calling onComplete");
                onComplete();
            }
        }

        void CurrentObjectives_UserObjectiveAdded(GameplayObjective objective)
        {
            ObjectiveListItem objectiveListItem = GameObject.Instantiate(_objectiveListItemPrefab) as ObjectiveListItem;
            objectiveListItem.ObjectiveName.text = objective.Name;
            objectiveListItem.transform.parent = _objectivePanel;
            objectiveListItem.transform.localScale = Vector3.one;

            objectiveListItem.Objective = objective;
        }

        public bool IsShowing
        {
            get { return _isShowing; }
        }

        // Unity button event to interface event mapping:
        private void BiologButtonPressedHandler()
        {
            if (BiologButtonPressed != null)
            {
                BiologButtonPressed();
            }
        }

        private void OptionsButtonPressedHandler()
        {
            if (OptionsButtonPressed != null)
            {
                OptionsButtonPressed();
            }
        }

        private void ResumeGameButtonPressedHandler()
        {
            if (ResumeGameButtonPressed != null)
            {
                ResumeGameButtonPressed();
            }
        }

        private void ExitGameButtonPressedHandler()
        {
            if (ExitGameButtonPressed != null)
            {
                ExitGameButtonPressed();
            }
        }
    }
}
