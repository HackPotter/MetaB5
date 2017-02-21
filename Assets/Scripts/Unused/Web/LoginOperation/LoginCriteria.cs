using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// LoginCriteria contains data required to execute a LoginOperation.
/// </summary>
public sealed class LoginCriteria
{
    /// <summary>
    /// The username for the login operation.
    /// </summary>
    public string Username
    {
        get;
        private set;
    }

    /// <summary>
    /// The password for the login operation.
    /// </summary>
    public string Password
    {
        get;
        private set;
    }

    //Private constructor. Don't mind me.
    private LoginCriteria(string username, string password)
    {
        Username = username;
        Password = password;
    }

    /// <summary>
    /// Creates a LoginCriteria with the given username and password.
    /// </summary>
    /// <param name="username">The username with which to attempt to login.</param>
    /// <param name="password">The password with which to attempt to login.</param>
    /// <returns></returns>
    public static LoginCriteria Create(string username, string password)
    {
        // Sanitize input?
        Asserter.NotNullOrEmpty(username);
        Asserter.NotNullOrEmpty(password);

        return new LoginCriteria(username, password);
    }
}
