using Squid;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MillionaireView : Frame {
    public event Action Help1ButtonPressed;
    public event Action Help2ButtonPressed;
    public event Action Help3ButtonPressed;
    public event Action AButtonPressed;
    public event Action BButtonPressed;
    public event Action CButtonPressed;
    public event Action DButtonPressed;
    public event Action YesButtonPressed;
    public event Action NoButtonPressed;
    public event Action ExitButtonPressed;

    public enum Colors { CORRECT = -16711936, INCORRECT = -36847, WHITE = -1, HIGHLIGHT = -256 }

    public MillionaireView(SquidLayout menuLayout) {
        Desktop menuDesktop = menuLayout.GetInstance();
        menuDesktop.Dock = DockStyle.Fill;
        Controls.Add(menuDesktop);

        (GetControl("Help1") as ImageControl).MouseClick += new MouseEvent(Help1Button_MouseClick);
        (GetControl("Help2") as ImageControl).MouseClick += new MouseEvent(Help2Button_MouseClick);
        (GetControl("Help3") as ImageControl).MouseClick += new MouseEvent(Help3Button_MouseClick);

        (GetControl("A") as Button).MouseClick += new MouseEvent(AButton_MouseClick);
        (GetControl("B") as Button).MouseClick += new MouseEvent(BButton_MouseClick);
        (GetControl("C") as Button).MouseClick += new MouseEvent(CButton_MouseClick);
        (GetControl("D") as Button).MouseClick += new MouseEvent(DButton_MouseClick);

        (GetControl("Yes") as Button).MouseClick += new MouseEvent(YesButton_MouseClick);
        (GetControl("No") as Button).MouseClick += new MouseEvent(NoButton_MouseClick);

        (GetControl("Exit") as Button).MouseClick += new MouseEvent(ExitButton_MouseClick);
    }

    void Help1Button_MouseClick(Control sender, MouseEventArgs args) {
        if (Help1ButtonPressed != null) {
            Help1ButtonPressed();
        }
    }

    void Help2Button_MouseClick(Control sender, MouseEventArgs args) {
        if (Help2ButtonPressed != null) {
            Help2ButtonPressed();
        }
    }

    void Help3Button_MouseClick(Control sender, MouseEventArgs args) {
        if (Help3ButtonPressed != null) {
            Help3ButtonPressed();
        }
    }

    void AButton_MouseClick(Control sender, MouseEventArgs args) {
        if (AButtonPressed != null) {
            AButtonPressed();
        }
    }

    void BButton_MouseClick(Control sender, MouseEventArgs args) {
        if (BButtonPressed != null) {
            BButtonPressed();
        }
    }

    void CButton_MouseClick(Control sender, MouseEventArgs args) {
        if (CButtonPressed != null) {
            CButtonPressed();
        }
    }

    void DButton_MouseClick(Control sender, MouseEventArgs args) {
        if (DButtonPressed != null) {
            DButtonPressed();
        }
    }

    void YesButton_MouseClick(Control sender, MouseEventArgs args) {
        if (YesButtonPressed != null) {
            YesButtonPressed();
        }
    }

    void NoButton_MouseClick(Control sender, MouseEventArgs args) {
        if (NoButtonPressed != null) {
            NoButtonPressed();
        }
    }

    void ExitButton_MouseClick(Control sender, MouseEventArgs args) {
        if (ExitButtonPressed != null) {
            ExitButtonPressed();
        }
    }

    public void EditLabel(string control, string text) {
        (GetControl(control) as Label).Text = text;
    }

    public void EditButton(string control, string text) {
        (GetControl(control) as Button).Text = text;
    }

    public void Highlight(int level, bool highlight) {
        //Hightlight current level
        if (highlight)
            (GetControl(level.ToString()) as FlowLayoutFrame).Tint = (int)Colors.HIGHLIGHT;
        else
            (GetControl(level.ToString()) as FlowLayoutFrame).Tint = (int)Colors.WHITE;
        //Remove highlight from previous level
        /*if (level > 1)
            (GetControl((level - 1).ToString()) as FlowLayoutFrame).Tint = (int)Colors.WHITE;*/
    }

    public void EnableAnswer(string control) {
        (GetControl(control) as Button).Enabled = true;
        (GetControl(control) as Button).Opacity = 1.0f;
    }

    public void EnableAllAnswers() {
        EnableAnswer("A");
        EnableAnswer("B");
        EnableAnswer("C");
        EnableAnswer("D");
    }

    public void EnableHelp(int help_number, string texture_path) {
        (GetControl("Help" + help_number) as ImageControl).Enabled = true;
        SetTexture("Help" + help_number, texture_path);

    }

    public void DisableAnswer(string control) {
        (GetControl(control) as Button).Enabled = false;
        (GetControl(control) as Button).Opacity = 0.5f;
    }

    public void DisableAllAnswers() {
        DisableAnswer("A");
        DisableAnswer("B");
        DisableAnswer("C");
        DisableAnswer("D");
    }

    public void DisableHelp(int help_number, string texture_path) {
        (GetControl("Help" + help_number) as ImageControl).Enabled = false;
        SetTexture("Help" + help_number, texture_path);
    }

    public void DisableButton(string control) {
        (GetControl(control) as Button).Visible = false;
        (GetControl(control) as Button).Enabled = false;
    }

    public void ButtonFrameVisibility(string control, bool visible) {
        (GetControl(control) as FlowLayoutFrame).Visible = visible;
    }

    public void LabelVisibility(string control, bool visible) {
        (GetControl(control) as Label).Visible = visible;
    }

    public void ImageVisibility(string control, bool visible) {
        (GetControl(control) as ImageControl).Visible = visible;
    }

    public void TextColor(string control, Colors color) {
        (GetControl(control) as Label).TextColor = (int)color;
    }

    public void SetTexture(string control, string texture_path) {
        (GetControl(control) as ImageControl).Texture = texture_path;
    }

    public void SetTitle(string name) {
        (GetControl("Title") as Label).Text = name;
    }
}

public class MillionaireUI : GuiRenderer {

#pragma warning disable 0067, 0649
    [SerializeField]
    private SquidLayout _launchScreen;
#pragma warning restore 0067, 0649

    private Millionaire _game;

    private enum Mode { IDLE, CONFIRM_ANSWER, CONFIRM_CONTINUE, CONFIRM_HELP, IMAGE, GAME_OVER, HELP }
    private Mode mode;

    private enum Answer { A = 1, B = 2, C = 3, D = 4 }
    private Answer answer;

    private enum Help { ELIMINATE = 1, IMAGE = 2, SKIP = 3 }
    private Help help_number;

    private int[] wrong_answers;
    private bool using_lifeline = false;

    private string gameTitle;
    private string instructions;
    private string[] answers;
    private string[] enable_help_path = { "Half_MillionaireIcon", "Image_MillionaireIcon", "QuestionMark_MillionaireIcon" };
    private string[] disable_help_path = { "HalfX_MillionaireIcon", "ImageX_MillionaireIcon", "QuestionMarkX_MillionaireIcon" };

    private bool game_lost = false;

    public MillionaireView view {
        get;
        private set;
    }

    public event Action UserQuitGame;

    public void StartGame(Millionaire game) {
        _game = game;
        gameTitle = _game.QuestionSetName;
        this.gameObject.SetActive(true);
    }

    protected override void Awake() {
        base.Awake();

        // Ok so this can be changed to take a list of Millionaire components.
        view = new MillionaireView(_launchScreen);
        view.Dock = DockStyle.Fill;

        Desktop.Controls.Add(view);



        instructions = "Please read the question carefully and select the best answer.\nIf you are unsure of the answer, you may click on one of the 3 'lifeline' buttons on the left.\nEach button works only once.";

        if (SceneManager.sceneCountInBuildSettings == 1)
            view.DisableButton("Exit");

        view.Help1ButtonPressed += new System.Action(MillionaireView_Help1ButtonPressed);
        view.Help2ButtonPressed += new System.Action(MillionaireView_Help2ButtonPressed);
        view.Help3ButtonPressed += new System.Action(MillionaireView_Help3ButtonPressed);

        view.YesButtonPressed += new System.Action(MillionaireView_YesButtonPressed);
        view.NoButtonPressed += new System.Action(MillionaireView_NoButtonPressed);

        view.AButtonPressed += new System.Action(MillionaireView_AButtonPressed);
        view.BButtonPressed += new System.Action(MillionaireView_BButtonPressed);
        view.CButtonPressed += new System.Action(MillionaireView_CButtonPressed);
        view.DButtonPressed += new System.Action(MillionaireView_DButtonPressed);

        view.ExitButtonPressed += new System.Action(MillionaireView_ExitButtonPressed);

        mode = Mode.IDLE;
    }

    protected override void OnGUI() {
        base.OnGUI();

        answers = _game.AvailableAnswers;

        view.SetTitle(gameTitle);

        for (int i = 1; i <= _game.MaxLevel; i++) {
            if (i == _game.CurrentLevel)
                view.Highlight(i, true);
            else
                view.Highlight(i, false);
        }

        view.EditButton("A", "A. " + answers[0]);
        view.EditButton("B", "B. " + answers[1]);
        view.EditButton("C", "C. " + answers[2]);
        view.EditButton("D", "D. " + answers[3]);

        if (wrong_answers != null)
            disable_answers();

        if (mode == Mode.IDLE) {
            view.EditLabel("Confirm", instructions);
            view.EditLabel("Question", _game.CurrentQuestionText);

            view.TextColor("Question", MillionaireView.Colors.WHITE);

            if (using_lifeline)
                view.LabelVisibility("Correct", true);
            else
                view.LabelVisibility("Correct", false);

            view.LabelVisibility("Confirm", true);
            view.ImageVisibility("ImageClue", false);
            view.ButtonFrameVisibility("YesNo", false);
            view.ButtonFrameVisibility("Answers", true);
        }
        else if (mode == Mode.IMAGE) {
            view.TextColor("Question", MillionaireView.Colors.WHITE);
            view.LabelVisibility("Correct", false);
            view.LabelVisibility("Confirm", false);
            view.ImageVisibility("ImageClue", true);
            view.ButtonFrameVisibility("YesNo", false);
            view.ButtonFrameVisibility("Answers", true);
        }
        else if (mode == Mode.CONFIRM_ANSWER) {
            view.EditLabel("Confirm", "Is '" + to_letter_answer((int)answer) + "' your final answer?");
            view.EditButton("Yes", "YES");
            view.EditButton("No", "NO");

            view.TextColor("Question", MillionaireView.Colors.WHITE);
            view.LabelVisibility("Correct", false);
            view.LabelVisibility("Confirm", true);
            view.ImageVisibility("ImageClue", false);
            view.ButtonFrameVisibility("YesNo", true);
            view.ButtonFrameVisibility("Answers", true);
            view.DisableAllAnswers();
        }
        else if (mode == Mode.CONFIRM_CONTINUE) {
            view.EditLabel("Question", "Total Points: " + _game.CurrentScore + " nMol");
            view.EditLabel("Confirm", "Would you like to continue playing or walk away with half of your points?");
            view.EditButton("Yes", "CONTINUE");
            view.EditButton("No", "WALK AWAY");

            view.TextColor("Question", MillionaireView.Colors.HIGHLIGHT);
            view.LabelVisibility("Correct", true);
            view.LabelVisibility("Confirm", true);
            view.ImageVisibility("ImageClue", false);
            view.ButtonFrameVisibility("YesNo", true);
            view.ButtonFrameVisibility("Answers", false);
        }
        else if (mode == Mode.CONFIRM_HELP) {
            view.EditLabel("Confirm", "Do you want to use this lifeline?");
            view.EditButton("Yes", "YES");
            view.EditButton("No", "NO");

            view.TextColor("Correct", MillionaireView.Colors.CORRECT);
            view.LabelVisibility("Correct", true);
            view.LabelVisibility("Confirm", true);
            view.ImageVisibility("ImageClue", false);
            view.ButtonFrameVisibility("YesNo", true);
            view.ButtonFrameVisibility("Answers", true);
        }
        else {
            view.EditLabel("Question", "You have earned a total of " + _game.CurrentScore + " nmol!");
            view.EditLabel("Confirm", "Would you like to play again or exit the game?");
            //view.EditLabel("Confirm", "Please click 'Close' to exit the game.");
            view.EditButton("Yes", "PLAY AGAIN");
            view.EditButton("No", "EXIT");

            view.TextColor("Question", MillionaireView.Colors.HIGHLIGHT);
            view.LabelVisibility("Correct", true);
            view.LabelVisibility("Confirm", true);
            view.ImageVisibility("ImageClue", false);
            view.ButtonFrameVisibility("YesNo", true);
            if (!game_lost)
                view.ButtonFrameVisibility("Answers", false);
            else {
                view.ButtonFrameVisibility("Answers", true);
                view.DisableAllAnswers();
            }
        }
    }

    string to_letter_answer(int num_answer) {
        string letter = null;

        switch (num_answer) {
            case 1: letter = "A"; break;
            case 2: letter = "B"; break;
            case 3: letter = "C"; break;
            case 4: letter = "D"; break;
        }

        return letter;
    }

    void MillionaireView_Help1ButtonPressed() {
        //Prevent player from using more than one lifeline at a time
        if (using_lifeline) {
            //Doesn't show message if player hasn't confirmed the previous lifeline
            if (mode != Mode.CONFIRM_HELP) {
                view.EditLabel("Correct", "You can't use more than one lifeline per question");
                view.TextColor("Correct", MillionaireView.Colors.INCORRECT);
            }
        }
        else if (mode == Mode.IDLE) {
            help_number = Help.ELIMINATE;
            using_lifeline = true;
            mode = Mode.CONFIRM_HELP;
            view.EditLabel("Correct", "50/50 \nThis lifeline eliminates two of the incorrect answer choices.");
        }
    }

    void MillionaireView_Help2ButtonPressed() {
        //Prevent player from using more than one lifeline at a time
        if (using_lifeline) {
            //Doesn't show message if player hasn't confirmed the previous lifeline
            if (mode != Mode.CONFIRM_HELP) {
                view.EditLabel("Correct", "You can't use more than one lifeline per question");
                view.TextColor("Correct", MillionaireView.Colors.INCORRECT);
            }
        }
        else if (mode == Mode.IDLE) {
            help_number = Help.IMAGE;
            using_lifeline = true;
            mode = Mode.CONFIRM_HELP;
            view.EditLabel("Correct", "CLUE \nThis lifeline displays an image which contains clues to the answer.");
        }
    }

    void MillionaireView_Help3ButtonPressed() {
        //Prevent player from using more than one lifeline at a time
        if (using_lifeline) {
            //Doesn't show message if player hasn't confirmed the previous lifeline
            if (mode != Mode.CONFIRM_HELP) {
                view.EditLabel("Correct", "You can't use more than one lifeline per question");
                view.TextColor("Correct", MillionaireView.Colors.INCORRECT);
            }
        }
        else if (mode == Mode.IDLE) {
            help_number = Help.SKIP;
            using_lifeline = true;
            mode = Mode.CONFIRM_HELP;
            view.EditLabel("Correct", "Switch \nThis lifeline allows you to switch this question for another one.");
        }
    }

    void MillionaireView_AButtonPressed() {
        answer = Answer.A;
        mode = Mode.CONFIRM_ANSWER;
    }

    void MillionaireView_BButtonPressed() {
        answer = Answer.B;
        mode = Mode.CONFIRM_ANSWER;
    }

    void MillionaireView_CButtonPressed() {
        answer = Answer.C;
        mode = Mode.CONFIRM_ANSWER;
    }

    void MillionaireView_DButtonPressed() {
        answer = Answer.D;
        mode = Mode.CONFIRM_ANSWER;
    }

    void MillionaireView_YesButtonPressed() {
        if (mode == Mode.CONFIRM_ANSWER) {
            mode = Mode.CONFIRM_CONTINUE;
            using_lifeline = false;
            check_answer();
        }
        else if (mode == Mode.CONFIRM_CONTINUE) {
            mode = Mode.IDLE;
            _game.NextLevel();

            game_lost = false;
            wrong_answers = null;
            using_lifeline = false;
            view.EnableAllAnswers();
        }
        else if (mode == Mode.CONFIRM_HELP) {
            mode = Mode.IDLE;
            view.DisableHelp((int)help_number, disable_help_path[(int)help_number - 1]);

            if (help_number == Help.ELIMINATE) {
                wrong_answers = _game.Eliminate();
            }
            else if (help_number == Help.IMAGE) {
                mode = Mode.IMAGE;
                view.SetTexture("ImageClue", _game.CurrentImagePath());
            }
            else {
                _game.SwitchQuestion();
            }
        }
        else if (mode == Mode.GAME_OVER) {
            mode = Mode.IDLE;
            using_lifeline = false;
            _game.ResetGame();
            wrong_answers = null;
            game_lost = false;
            view.EnableAllAnswers();
            view.EnableHelp(1, enable_help_path[0]);
            view.EnableHelp(2, enable_help_path[1]);
            view.EnableHelp(3, enable_help_path[2]);
        }
    }

    void MillionaireView_NoButtonPressed() {
        if (mode == Mode.CONFIRM_ANSWER || mode == Mode.CONFIRM_HELP) {
            if (mode == Mode.CONFIRM_HELP)
                using_lifeline = false;

            mode = Mode.IDLE;
            view.EnableAllAnswers();
        }
        else if (mode == Mode.CONFIRM_CONTINUE) {
            mode = Mode.GAME_OVER;
            view.EditLabel("Correct", "");
            _game.WalkAway();
        }
        else if (mode == Mode.GAME_OVER) {

            if (SceneManager.sceneCountInBuildSettings == 1)
                Application.Quit();
            else
                MillionaireView_ExitButtonPressed();
        }
    }

    void MillionaireView_ExitButtonPressed() {
        //Add total points from all games to player's score, then return to lab.
        //GameContext.Instance.Player.Points = game.get_final_score();
        //Application.LoadLevel("Laboratory");
        //this.gameObject.SetActive(false);
        if (UserQuitGame != null) {
            UserQuitGame();
            GameObject.Destroy(this.gameObject);
        }
    }

    void check_answer() {
        if (_game.SubmitAnswer((int)answer)) {
            view.TextColor("Correct", MillionaireView.Colors.CORRECT);

            if (_game.CurrentLevel == _game.MaxLevel) {
                mode = Mode.GAME_OVER;
                view.EditLabel("Correct", "CORRECT! \nCongratulations! You have won the game!");
            }
            else {
                view.EditLabel("Correct", "CORRECT!");
            }
        }
        else {
            mode = Mode.GAME_OVER;
            game_lost = true;
            view.EditLabel("Correct", "INCORRECT \n The correct answer is " + to_letter_answer(_game.GetCorrectAnswer()) + ".\nUnfortunately this means the game is over.");
            view.TextColor("Correct", MillionaireView.Colors.INCORRECT);
        }
    }

    void disable_answers() {
        foreach (int wrong_answer in wrong_answers) {
            switch (wrong_answer) {
                case (int)Answer.A: view.DisableAnswer("A"); break;
                case (int)Answer.B: view.DisableAnswer("B"); break;
                case (int)Answer.C: view.DisableAnswer("C"); break;
                case (int)Answer.D: view.DisableAnswer("D"); break;
            }
        }
    }
}