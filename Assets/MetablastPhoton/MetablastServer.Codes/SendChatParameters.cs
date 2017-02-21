using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetablastServer.Codes
{
    public enum SendChatParameters : byte
    {
        Receiver = 0,
        Message = 1,
        Sender = 2,
    }
}
