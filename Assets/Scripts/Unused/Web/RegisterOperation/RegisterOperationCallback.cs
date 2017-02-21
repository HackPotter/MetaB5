using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Delegate for RegisterOperation asynchronous callbacks.
/// </summary>
/// <param name="result">The result of the RegisterOperation (non-null).</param>
public delegate void RegisterOperationCallback(RegisterOperationResult result);