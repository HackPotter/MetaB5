using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Metablast.UI {
    public class NewBiologUI : UIScreen, IBiologView {
#pragma warning disable 0067, 0649
        [SerializeField]
        private AnimationClip _showAnimation;
        [SerializeField]
        private Image _tintImage;
        [SerializeField]
        private BiologEntryButtonPressed _buttonPressed;
        [SerializeField]
        private BiologEntryListElement _listElementPrefab;
        [SerializeField]
        private VerticalLayoutGroup _databaseList;
        [SerializeField]
        private InputField _searchField;
        [SerializeField]
        private Text _databaseText;
        [SerializeField]
        private Button _exitButton;
        [SerializeField]
        private Animation _animation;
#pragma warning restore 0067, 0649

        private bool _isShowing;

        private List<BiologEntryListElement> _currentListElements = new List<BiologEntryListElement>();

        void Start() {
            _showAnimation.SampleAnimation(gameObject, 0f);

            BuildEntryList();

            _exitButton.onClick.AddListener(ExitButtonOnClickHandler);
            _searchField.onValueChanged.AddListener(searchFieldChangedHandler);
        }

        void searchFieldChangedHandler(string searchString) {
            for (int i = 0; i < _currentListElements.Count; i++) {
                BiologEntryListElement listElement = _currentListElements[i];

                if (listElement.Entry.EntryName.ToLower().Contains(searchString.Trim().ToLower())) {
                    listElement.gameObject.SetActive(true);
                    continue;
                }

                bool matches = false;
                foreach (string tag in listElement.Entry.Tags) {
                    if (tag.Trim().ToLower().Contains(searchString.Trim().ToLower())) {
                        matches = true;
                        break;
                    }
                }
                listElement.gameObject.SetActive(matches);
            }
        }

        void ExitButtonOnClickHandler() {
            if (ExitButtonPressed != null) {
                ExitButtonPressed();
            }
        }

        void OnElementClick(BiologEntryListElement entryElement) {
            SetBiologEntry(entryElement.Entry);
            _buttonPressed.Invoke(entryElement);
        }

        public void SetBiologEntry(BiologEntry entry) {
            _databaseText.text = entry.DescriptionText;
        }

        public void RefreshEntries() {
            for (int i = 0; i < _currentListElements.Count; i++) {
                GameObject.Destroy(_currentListElements[i].gameObject);
            }
            _currentListElements.Clear();
            BuildEntryList();
        }

        private void BuildEntryList() {
            foreach (BiologEntry entry in GameContext.Instance.Player.BiologProgress.UnlockedEntries) {
                BiologEntryListElement entryElement = (BiologEntryListElement)GameObject.Instantiate(_listElementPrefab, _databaseList.transform.position, Quaternion.identity);
                entryElement.transform.SetParent(_databaseList.transform);
                entryElement.transform.localScale = Vector3.one;
                entryElement.Entry = entry;
                entryElement.ButtonPressed.AddListener(OnElementClick);
                _currentListElements.Add(entryElement);
            }
        }

        public event Action ExitButtonPressed;

        public bool IsShowing {
            get { return _isShowing; }
        }

        public void SetTint(float tint) {
            // Brad 3/1/17
            //Color newColor = _tintImage.color;
            //newColor.a = tint;
            //_tintImage.color = newColor;
        }

        public override void Show(Action onComplete, float animationTime = 0.4f) {
            _animation.clip = _showAnimation;
            _animation[_showAnimation.name].speed = 1f;
            StartCoroutine(PlayAnimationCoroutine(_showAnimation, () => {
                _isShowing = true;
                if (onComplete != null) onComplete();
            }));
        }

        public override void Hide(Action onComplete, float animationTime = 0.4f) {
            _animation.clip = _showAnimation;
            if (!_animation.isPlaying) {
                _animation[_showAnimation.name].time = _showAnimation.length;
            }

            _animation[_showAnimation.name].speed = -1f;
            StartCoroutine(PlayAnimationCoroutine(_showAnimation, () => {
                _isShowing = false;
                if (onComplete != null) onComplete();
            }));
        }

        private IEnumerator PlayAnimationCoroutine(AnimationClip animationClip, Action onComplete) {
            _animation.Play(animationClip.name, PlayMode.StopAll);
            while (_animation.isPlaying) {
                yield return null;
            }

            if (onComplete != null) {
                onComplete();
            }
        }
    }
}