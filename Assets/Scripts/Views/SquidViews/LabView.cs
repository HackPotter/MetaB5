using Squid;
using UnityEngine.SceneManagement;

public class LabView {
    private Button _exitGameButton;
    private Frame _helpFrame;

    public LabView(Desktop desktop) {
        _helpFrame = desktop.GetControl("HelpFrame") as Frame;
        _exitGameButton = desktop.GetControl("Exit Game") as Button;

        _exitGameButton.MouseClick += new MouseEvent(_exitGameButton_MouseClick);


        Button helpButton = (Button)desktop.GetControl("HelpButton");
        Control helpFrame = desktop.GetControl("HelpFrame");
        helpButton.MouseClick += (sender, args) => { helpFrame.Visible = !helpFrame.Visible; };
        Button closeHelpButton = (Button)desktop.GetControl("CloseHelpButton");
        closeHelpButton.MouseClick += (sender, args) => { helpFrame.Visible = false; };
    }

    void _helpButton_MouseClick(Control sender, MouseEventArgs args) {
        _helpFrame.Visible = !_helpFrame.Visible;
    }

    void _exitGameButton_MouseClick(Control sender, MouseEventArgs args) {
        SceneManager.LoadScene("MainMenu");
    }
}

