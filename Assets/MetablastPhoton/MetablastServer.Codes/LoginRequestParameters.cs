using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetablastServer.Codes
{
    public enum LoginRequestParameters : byte
    {
        Email = 0,
        Password = 1,
    }

    public enum LoginUserErrorDetail : byte
    {
        InvalidCredentials = 0,
        UserAlreadyLoggedIn = 1,
    }
}
