using System;
using Squid;

public class LoginView : Frame, ILoginView
{
    public event Action LoginButtonPressed;
    public event Action ResetPasswordButtonPressed;
    public event Action RegisterButtonPressed;
    public event Action BackButtonPressed;

    private TextBox _emailTextBox;
    private TextBox _passwordTextBox;

    public string Email
    {
        get { return _emailTextBox.Text; }
    }

    public string Password
    {
        get { return _passwordTextBox.Text; }
    }

    public LoginView(SquidLayout layout)
    {
        Desktop loginDesktop = layout.GetInstance();
        loginDesktop.Dock = DockStyle.Fill;

        Controls.Add(loginDesktop);


        (loginDesktop.GetControl("ResetPassword") as Button).MouseClick += new MouseEvent(ResetPasswordButton);

        (loginDesktop.GetControl("GoButton") as Button).MouseClick +=new MouseEvent(LoginButton);


        (loginDesktop.GetControl("RegisterLink") as Button).MouseClick += new MouseEvent(RegisterButton);

        (loginDesktop.GetControl("BackButton") as Button).MouseClick += new MouseEvent(BackButton);

        _emailTextBox = (loginDesktop.GetControl("EmailTextBox") as TextBox);
        _passwordTextBox = (loginDesktop.GetControl("PasswordTextBox") as TextBox);


        _emailTextBox.MouseClick += new MouseEvent(_emailTextBox_MouseClick);
        _emailTextBox.LostFocus += new VoidEvent(_emailTextBox_LostFocus);
        _passwordTextBox.MouseClick += new MouseEvent(_passwordTextBox_MouseClick);
        _passwordTextBox.LostFocus += new VoidEvent(_passwordTextBox_LostFocus);
    }

    void _emailTextBox_MouseClick(Control sender, MouseEventArgs args)
    {
        if (_emailTextBox.Text == "example@email.com")
        {
            _emailTextBox.Text = "";
        }
    }

    void _emailTextBox_LostFocus(Control sender)
    {
        if (_emailTextBox.Text == "")
        {
            _emailTextBox.Text = "example@email.com";
        }
    }

    void _passwordTextBox_MouseClick(Control sender, MouseEventArgs args)
    {
        if (_passwordTextBox.Text == "Password")
        {
            _passwordTextBox.Text = "";
        }
    }

    void _passwordTextBox_LostFocus(Control sender)
    {
        if (_passwordTextBox.Text == "")
        {
            _passwordTextBox.Text = "Password";
        }
    }

    void BackButton(Control sender, MouseEventArgs args)
    {
        if (BackButtonPressed != null)
        {
            BackButtonPressed();
        }
    }

    void RegisterButton(Control sender, MouseEventArgs args)
    {
        if (RegisterButtonPressed != null)
        {
            RegisterButtonPressed();
        }
    }

    void LoginButton(Control sender, MouseEventArgs args)
    {
        if (LoginButtonPressed != null)
        {
            LoginButtonPressed();
        }
    }

    void ResetPasswordButton(Control sender, MouseEventArgs args)
    {
        if (ResetPasswordButtonPressed != null)
        {
            ResetPasswordButtonPressed();
        }
    }
}

