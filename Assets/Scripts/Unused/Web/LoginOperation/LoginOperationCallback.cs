using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Delegate for LoginOperation asynchronous callbacks.
/// </summary>
/// <param name="result">The result of the LoginOperation (non-null).</param>
public delegate void LoginOperationCallback(LoginOperationResult result);
