using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Delegate for WebOperation asynchronous callbacks.
/// </summary>
/// <param name="result">The result of the WebOperation, or null if the WebOperation was passed an invalid WebOperationCriteria.</param>
public delegate void WebOperationCallback(ResultSet result);
