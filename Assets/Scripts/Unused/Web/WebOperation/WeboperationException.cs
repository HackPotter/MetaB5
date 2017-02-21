using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class WebOperationException : Exception
{
    public ResultSetErrorDetails ErrorDetails
    {
        get;
        private set;
    }

    public WebOperationException(ResultSetErrorDetails details)
    {
        ErrorDetails = details;
    }
}

