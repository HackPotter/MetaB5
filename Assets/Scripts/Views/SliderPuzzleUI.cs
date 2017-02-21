using Squid;
using System;
using UnityEngine;

public class SliderView : Frame
{
    public event Action PlayButtonPressed;
    public event Action NewGameButtonPressed;
    public event Action ExitButtonPressed;

    public SliderView(SquidLayout menuLayout)
    {
        Desktop menuDesktop = menuLayout.GetInstance();
        menuDesktop.Dock = DockStyle.Fill;
        Controls.Add(menuDesktop);

        (GetControl("Start") as Button).MouseClick += new MouseEvent(PlayButton_MouseClick);
        (GetControl("New") as Button).MouseClick += new MouseEvent(NewGameButton_MouseClick);
        (GetControl("Exit") as Button).MouseClick += new MouseEvent(ExitButton_MouseClick);

    }

    public void EditStartButton(String text)
    {
        (GetControl("Start") as Button).Text = text;
    }

    public void setScore(int score)
    {
        (GetControl("Score") as TextBox).Text = score.ToString();
    }

    void ExitButton_MouseClick(Control sender, MouseEventArgs args)
    {
        if (ExitButtonPressed != null)
        {
            ExitButtonPressed();
        }
    }

    void PlayButton_MouseClick(Control sender, MouseEventArgs args)
    {
        if (PlayButtonPressed != null)
        {
            PlayButtonPressed();
        }
    }

    void NewGameButton_MouseClick(Control sender, MouseEventArgs args)
    {
        if (NewGameButtonPressed != null)
        {
            NewGameButtonPressed();
        }
    }
}


public class SliderPuzzleUI : GuiRenderer
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private SquidLayout _launchScreen;
#pragma warning restore 0067, 0649
    private TileSlider game;
    private bool play_button_is_start;

    public SliderView view
    {
        get;
        private set;
    }

    protected override void Awake()
    {
        base.Awake();

        view = new SliderView(_launchScreen);
        view.Dock = DockStyle.Fill;

        play_button_is_start = true;

        game = GameObject.Find("slider").GetComponent<TileSlider>();

        Desktop.Controls.Add(view);

        view.ExitButtonPressed += new System.Action(SliderView_ExitButtonPressed);
        view.PlayButtonPressed += new System.Action(SliderView_PlayButtonPressed);
        view.NewGameButtonPressed += new System.Action(SliderView_NewGameButtonPressed);
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        view.setScore(game.get_score());
    }

    void SliderView_PlayButtonPressed()
    {
        if (play_button_is_start)
        {
            play_button_is_start = false;
            SliderView_StartButtonPressed();
        }
        else
        {
            SliderView_ResetButtonPressed();
        }
    }

    void SliderView_StartButtonPressed()
    {
        game.start_game();
        view.EditStartButton("RESET");
    }

    void SliderView_ResetButtonPressed()
    {
        game.reset();
    }

    void SliderView_NewGameButtonPressed()
    {
        game.new_game();
        view.EditStartButton("START");
        play_button_is_start = true;
    }

    void SliderView_ExitButtonPressed()
    {
        Application.LoadLevel("Laboratory");

    }
}