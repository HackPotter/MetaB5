using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Interface for condition verifiers.
/// <br/>
/// 
/// A condition verifier is used to assert that specific conditions hold true in the editor.
/// </summary>
public interface IConditionVerifier
{
    /// <summary>
    /// Verifies that a condition is true on the given GameObject, or returns a List of FieldViolations with details about the violation.
    /// </summary>
    /// <param name="gameObject">The GameObject on which to verify a condition.</param>
    /// <returns>A List of FieldViolations if the given GameObject violates the condition, or an empty list if no conditions are violated.</returns>
    List<FieldViolation> VerifyCondition(GameObject gameObject);
}

