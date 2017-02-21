using System;
using Squid;
using UnityEngine;

public class MenuView : Frame, IMenuView
{
    public event Action PlayButtonPressed;
    public event Action LoginButtonPressed;
    public event Action ExitButtonPressed;

    public MenuView(SquidLayout menuLayout)
    {
        Desktop menuDesktop = menuLayout.GetInstance();
        menuDesktop.Dock = DockStyle.Fill;
        Controls.Add(menuDesktop);

        (GetControl("SinglePlayerButton") as Button).MouseClick += new MouseEvent(PlayButton_MouseClick);
        (GetControl("LoginButton") as Button).MouseClick += new MouseEvent(MenuView_MouseClick);
        (GetControl("ExitButton") as Button).MouseClick += new MouseEvent(ExitButton_MouseClick);
    }

    void MenuView_MouseClick(Control sender, MouseEventArgs args)
    {
        if (LoginButtonPressed != null)
        {
            LoginButtonPressed();
        }
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
}

