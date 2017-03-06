using System.Linq;
using UnityEngine;

public class Millionaire {
    private int _currentQuestionSet = 0;
    private int _currentLevel = 0;
    private int _finalScore = 0;
    private int _currentScore = 0;
    private int _currentQuestionIndex;
    private int _correctAnswerIndex;
    private int[] _scores = { 1, 2, 3, 4, 5, 10, 15, 20, 25, 30, 130, 140, 155, 175, 200 };

    private QuestionSet _questionSet;

    public Millionaire(QuestionSet questionSet) {
        _questionSet = questionSet;
        ResetGame();

    }

    private void change_question() {
        int question_id;

        do {
            question_id = Random.Range(1, 6);
        } while (_currentQuestionIndex == question_id);

        _currentQuestionIndex = question_id;
    }

    public int GetQuestionSetIndex() {
        return _currentQuestionSet;
    }

    public int CurrentLevel {
        get { return _currentLevel; }
    }

    public int MaxLevel {
        get { return 15; }
    }

    public string CurrentQuestionText {
        get { return _questionSet.Levels[_currentLevel - 1].questions[_currentQuestionIndex - 1].content; }
    }

    public string[] AvailableAnswers {
        get {
            return _questionSet.Levels[_currentLevel - 1].questions[_currentQuestionIndex - 1].answers.Select((q) => q.content).ToArray();
        }
    }

    public int CurrentScore {
        get { return _currentScore; }
    }

    public int FinalScore {
        get { return _currentScore; }
    }

    public string CurrentImagePath() {
        return _questionSet.Levels[_currentLevel - 1].questions[_currentQuestionIndex - 1].imagePath;
    }

    public double GetImgWidth() {
        return _questionSet.Levels[_currentLevel - 1].questions[_currentQuestionIndex - 1].imgWidth;
    }

    public double GetImgHeight() {
        return _questionSet.Levels[_currentLevel - 1].questions[_currentQuestionIndex - 1].imgHeight;
    }

    public string QuestionSetName {
        get {
            return _questionSet.Title;
        }
    }

    public void NextLevel() {
        _currentLevel++;
        _currentQuestionIndex = Random.Range(1, 1 + _questionSet.Levels[_currentLevel - 1].questions.Count());

        GetCorrectAnswer();
    }

    public void SwitchQuestion() {
        int temp;

        do {
            temp = Random.Range(1, 1 + _questionSet.Levels[_currentLevel - 1].questions.Count());
        } while (_currentQuestionIndex == temp);

        _currentQuestionIndex = temp;

        GetCorrectAnswer();
    }

    public int GetCorrectAnswer() {
        for (int i = 0; i < 4; i++) {
            if (_questionSet.Levels[_currentLevel - 1].questions[_currentQuestionIndex - 1].answers[i].correct) {
                _correctAnswerIndex = i + 1;
                break;
            }
        }
        return _correctAnswerIndex;
    }

    public bool SubmitAnswer(int answer_id) {
        if (_questionSet.Levels[_currentLevel - 1].questions[_currentQuestionIndex - 1].answers[answer_id - 1].correct) {
            if (_currentLevel < 11)
                _currentScore += _scores[_currentLevel - 1];
            else
                _currentScore = _scores[_currentLevel - 1];

            _finalScore += _currentScore;

            return true;
        }

        if (_currentLevel <= 5)
            _currentScore = 0;
        else if (_currentLevel <= 10)
            _currentScore = _scores[4];
        else
            _currentScore = _scores[9];

        return false;
    }

    public int[] Eliminate() {
        int[] wrong = new int[2];

        do {
            wrong[0] = Random.Range(1, 5);
            wrong[1] = Random.Range(1, 5);
        }
        while (wrong[0] == wrong[1] || wrong[0] == _correctAnswerIndex || wrong[1] == _correctAnswerIndex);

        return wrong;
    }

    public void WalkAway() {
        _currentScore = Mathf.CeilToInt(_currentScore / 2.0f);
    }

    public void ResetGame() {
        _currentScore = 0;
        _currentLevel = 0;
        NextLevel();
    }
}