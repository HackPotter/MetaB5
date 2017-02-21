using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetablastServer.Codes
{
    public enum ResponseReturnCode : short
    {
        OK = 0,
        InvalidParameters = -1,
        Error = -2
    }
}
