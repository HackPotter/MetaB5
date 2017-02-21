using System;

public interface ILoginView
{
    event Action LoginButtonPressed;
    event Action ResetPasswordButtonPressed;
    event Action RegisterButtonPressed;
    event Action BackButtonPressed;

    string Email { get; }
    string Password { get; }
}

