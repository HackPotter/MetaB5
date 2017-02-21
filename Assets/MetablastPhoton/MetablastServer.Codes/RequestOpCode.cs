using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetablastServer.Codes
{
    public enum RequestOpCode : byte
    {
        Login = 0,
        Register = 1,
        WriteLogEntry = 2,
        WriteAnonymousLogEntry = 3,
        SendChat = 4,
    }
}
