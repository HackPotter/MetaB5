using System;
using Squid;
using UnityEngine;

public class MinigameView : Frame
{
    public event Action PlayButtonPressed;
    public event Action NewGameButtonPressed;
    public event Action ExitButtonPressed;

    public MinigameView(SquidLayout menuLayout)
    {
        Desktop menuDesktop = menuLayout.GetInstance();
        menuDesktop.Dock = DockStyle.Fill;
        Controls.Add(menuDesktop);

        (GetControl("Start") as Button).MouseClick += new MouseEvent(PlayButton_MouseClick);
        (GetControl("New") as Button).MouseClick += new MouseEvent(NewGameButton_MouseClick);
        (GetControl("ExitButton") as Button).MouseClick += new MouseEvent(ExitButton_MouseClick);

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