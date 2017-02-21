using UnityEngine;
using System.Collections;
using Squid;
using System;

public class RegisterView : Frame, IRegisterView
{
    public event Action BackButtonPressed;
    public event Action SubmitButtonPressed;

    private TextBox _emailTextBox;
    private TextBox _passwordTextBox;
    private TextBox _confirmPasswordTextBox;

    private TextBox _ageTextBox;
    private DropDownList _educationDropDown;

    private RadioButton _maleGenderRadio;
    private RadioButton _femaleGenderRadio;

    private RadioButton _collectDataYes;
    private RadioButton _collectDataNo;

    private Label _passwordsMustMatch;

    public string Email
    {
        get { return _emailTextBox.Text; }
    }

    public string Password
    {
        get { return _passwordTextBox.Text; }
    }

    public string ConfirmPassword
    {
        get { return _confirmPasswordTextBox.Text; }
    }

    public int Age
    {
        get { return int.Parse(_ageTextBox.Text); }
    }

    public string Gender
    {
        get { return _maleGenderRadio.Checked ? "Male" : "Female"; }
    }

    public bool CollectData
    {
        get { return _collectDataYes.Checked; }
    }

    public RegisterView(SquidLayout registerLayout)
    {
        Desktop registerDesktop = registerLayout.GetInstance();
        registerDesktop.Dock = DockStyle.Fill;

        Controls.Add(registerDesktop);

        (registerDesktop.GetControl("SubmitButton") as Button).MouseClick += new MouseEvent(SubmitButton);

        (registerDesktop.GetControl("BackButton") as Button).MouseClick += new MouseEvent(BackButton);

        _emailTextBox = (registerDesktop.GetControl("EmailTextBox") as TextBox);
        _passwordTextBox = (registerDesktop.GetControl("PasswordTextBox") as TextBox);
        _confirmPasswordTextBox = (registerDesktop.GetControl("ConfirmPasswordTextBox") as TextBox);

        _ageTextBox = (registerDesktop.GetControl("AgeTextBox") as TextBox);
        _educationDropDown = (registerDesktop.GetControl("EducationDropDown") as DropDownList);

        _maleGenderRadio = (registerDesktop.GetControl("GenderMaleRadio") as RadioButton);
        _femaleGenderRadio = (registerDesktop.GetControl("GenderFemaleRadio") as RadioButton);

        _collectDataYes = (registerDesktop.GetControl("CollectDataYesRadio") as RadioButton);
        _collectDataNo = (registerDesktop.GetControl("CollectDataNoRadio") as RadioButton);

        _passwordsMustMatch = (registerDesktop.GetControl("PasswordsMustMatchLabel") as Label);
        _passwordsMustMatch.Visible = false;

        _emailTextBox.MouseClick += new MouseEvent(_emailTextBox_MouseClick);
        _emailTextBox.LostFocus += new VoidEvent(_emailTextBox_LostFocus);

        _passwordTextBox.MouseClick += new MouseEvent(_passwordTextBox_MouseClick);
        _passwordTextBox.LostFocus +=new VoidEvent(_passwordTextBox_LostFocus);
        
        _confirmPasswordTextBox.MouseClick += new MouseEvent(_confirmPasswordTextBox_MouseClick);
        _confirmPasswordTextBox.LostFocus += new VoidEvent(_confirmPasswordTextBox_LostFocus);

        _ageTextBox.MouseClick += new MouseEvent(_ageTextBox_MouseClick);
        _ageTextBox.LostFocus += new VoidEvent(_ageTextBox_LostFocus);

        _maleGenderRadio.MouseClick += new MouseEvent(_maleGenderRadio_MouseClick);
        _femaleGenderRadio.MouseClick += new MouseEvent(_femaleGenderRadio_MouseClick);

        _collectDataYes.MouseClick += new MouseEvent(_collectDataYes_MouseClick);
        _collectDataNo.MouseClick += new MouseEvent(_collectDataNo_MouseClick);

        _passwordTextBox.TextChanged += new VoidEvent(_passwordTextBox_TextChanged);
        _confirmPasswordTextBox.TextChanged += new VoidEvent(_passwordTextBox_TextChanged);

        _ageTextBox.TextChanged += new VoidEvent(_ageTextBox_TextChanged);

        _educationDropDown.MouseClick += new MouseEvent(_educationDropDown_MouseClick);
    }

    void _educationDropDown_MouseClick(Control sender, MouseEventArgs args)
    {
        _educationDropDown.Open();
    }

    void _ageTextBox_TextChanged(Control sender)
    {
        if (_ageTextBox.Text != "Age" && _ageTextBox.Text.Length > 2)
        {
            _ageTextBox.Text = _ageTextBox.Text.Substring(0, 2);
        }
    }

    void _passwordTextBox_TextChanged(Control sender)
    {
        if (_passwordTextBox.Text != "Password" || _confirmPasswordTextBox.Text != "Password")
        {
            if (_passwordTextBox.Text == _confirmPasswordTextBox.Text)
            {
                _passwordsMustMatch.Visible = false;
            }
            else
            {
                _passwordsMustMatch.Visible = true;
            }
        }
    }

    void _collectDataYes_MouseClick(Control sender, MouseEventArgs args)
    {
        _collectDataYes.Checked = true;
        _collectDataNo.Checked = false;
    }

    void _collectDataNo_MouseClick(Control sender, MouseEventArgs args)
    {
        _collectDataNo.Checked = true;
        _collectDataYes.Checked = false;
    }

    void _femaleGenderRadio_MouseClick(Control sender, MouseEventArgs args)
    {
        _femaleGenderRadio.Checked = true;
        _maleGenderRadio.Checked = false;
    }

    void _maleGenderRadio_MouseClick(Control sender, MouseEventArgs args)
    {
        _maleGenderRadio.Checked = true;
        _femaleGenderRadio.Checked = false;
    }

    void _ageTextBox_MouseClick(Control sender, MouseEventArgs args)
    {
        if (_ageTextBox.Text == "Age")
        {
            _ageTextBox.Text = "";
        }
    }

    void _ageTextBox_LostFocus(Control sender)
    {
        if (_ageTextBox.Text == "")
        {
            _ageTextBox.Text = "Age";
        }
    }

    void _passwordTextBox_MouseClick(Control sender, MouseEventArgs args)
    {
        if (_confirmPasswordTextBox.Text == "Password")
        {
            _confirmPasswordTextBox.Text = "";
        }
        if (_passwordTextBox.Text == "Password")
        {
            _passwordTextBox.Text = "";
        }
    }

    void _confirmPasswordTextBox_MouseClick(Control sender, MouseEventArgs args)
    {
        if (_confirmPasswordTextBox.Text == "Password")
        {
            _confirmPasswordTextBox.Text = "";
        }
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
        if (_confirmPasswordTextBox.Text == "")
        {
            _confirmPasswordTextBox.Text = "Password";
        }
    }

    void _confirmPasswordTextBox_LostFocus(Control sender)
    {
        if (_passwordTextBox.Text == "")
        {
            _passwordTextBox.Text = "Password";
        }
        if (_confirmPasswordTextBox.Text == "")
        {
            _confirmPasswordTextBox.Text = "Password";
        }
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

    void BackButton(Control sender, MouseEventArgs args)
    {
        if (BackButtonPressed != null)
        {
            BackButtonPressed();
        }
    }

    void SubmitButton(Control sender, MouseEventArgs args)
    {
        if (SubmitButtonPressed != null)
        {
            SubmitButtonPressed();
        }
    }
}
