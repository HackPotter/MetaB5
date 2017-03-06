using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionSetSelectionMenu : MonoBehaviour {
#pragma warning disable 0067, 0649
    [SerializeField]
    private QuestionSetLoader _loader;

    [SerializeField]
    private QuestionSetUIElement _questionSetUIElementPrefab;

    [SerializeField]
    private RectTransform _questionSetArea;

    [SerializeField]
    private MillionaireUI _gameUI;

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private Button _backButton;
#pragma warning restore 0067, 0649

    private QuestionSetUIElement _selected = null;

    void Awake() {
        ToggleGroup toggleGroup = _questionSetArea.GetComponent<ToggleGroup>();

        bool first = true;
        foreach (var questionSet in _loader.QuestionSets) {

            QuestionSetUIElement element = Instantiate(_questionSetUIElementPrefab) as QuestionSetUIElement;
            element.TitleText = questionSet.Title;
            element.Description = questionSet.Description;
            element.transform.parent = _questionSetArea.transform;
            element.transform.localScale = Vector3.one;
            element.Toggle.group = toggleGroup;
            element.QuestionSet = questionSet;
            Texture2D previewTexture = Resources.Load(questionSet.PreviewImagePath) as Texture2D;
            element.PreviewTexture = previewTexture ?? element.PreviewTexture;


            element.Toggle.onValueChanged.AddListener(
                (val) => {
                    _selected = element;
                });

            if (first) {
                _selected = element;
                element.Toggle.isOn = true;
                first = false;
            }
        }

        _playButton.onClick.AddListener(StartGame);
        _backButton.onClick.AddListener(Quit);

    }

    void _gameUI_UserQuitGame() {
        this.transform.root.gameObject.SetActive(true);
    }

    void StartGame() {
        var game = Instantiate(_gameUI) as MillionaireUI;
        game.StartGame(new Millionaire(_selected.QuestionSet));
        game.UserQuitGame += new System.Action(_gameUI_UserQuitGame);
        this.transform.root.gameObject.SetActive(false);

    }

    void Quit() {
        SceneManager.LoadScene("Laboratory");
    }
}
