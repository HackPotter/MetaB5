using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Exception thrown when Asserter fails an assertion.
/// </summary>
public class AssertionFailureException : Exception
{
    /// <summary>
    /// Creates a new AssertionFailureException with the given message.
    /// </summary>
    /// <param name="message"></param>
    public AssertionFailureException(string message)
        : base(message)
    {
    }
}

