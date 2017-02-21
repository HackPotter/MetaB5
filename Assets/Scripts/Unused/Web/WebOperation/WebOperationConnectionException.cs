using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class WebOperationConnectionException : Exception
{
    public WebOperationConnectionException(string ErrorMessage)
        : base(ErrorMessage)
    {
    }
}
