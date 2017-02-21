using System;

public interface IRegisterView
{
    event Action BackButtonPressed;
    event Action SubmitButtonPressed;

    string Email { get; }
    string Password { get; }
    string ConfirmPassword { get; }
    int Age { get; }
    string Gender { get; }
    bool CollectData { get; }
}

