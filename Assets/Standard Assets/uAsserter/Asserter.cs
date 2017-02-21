using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

/// <summary>
/// Asserter allows run-time assertion of expected conditions.
/// </summary>
public static class Asserter
{
    /// <summary>
    /// Asserts that the specified object is not null. If the object is a UnityEngine.Object, also checks the special Unity null condition.
    /// < br/>< br/>
    /// </summary>
    /// <param name="obj">The object to be asserted.</param>
    public static void NotNull(object obj)
    {
        if (obj == null || (obj is UnityEngine.Object && !(obj as UnityEngine.Object)))
        {
            throw new AssertionFailureException("Asserter.NotNull:Object is null.");
        }
    }

    /// <summary>
    /// Asserts that the specified object is not null and includes the given message in the exception. If the object is a UnityEngine.Object, also checks the special Unity null condition.
    /// </summary>
    /// <param name="obj">The object to be asserted.</param>
    /// <param name="message">A message to include in the thrown exception.</param>
    public static void NotNull(object obj, string message)
    {
        if (obj == null || (obj is UnityEngine.Object && !(obj as UnityEngine.Object)))
        {
            throw new AssertionFailureException("Asserter.NotNull:" + message);
        }
    }

    /// <summary>
    /// Asserts that the specified string is neither null nor empty.
    /// </summary>
    /// <param name="str">The string to assert.</param>
    public static void NotNullOrEmpty(string str)
    {
        if (String.IsNullOrEmpty(str))
        {
            throw new AssertionFailureException("Asserter.NotNullOrEmpty:String is null or empty");
        }
    }

    /// <summary>
    /// Asserts that the specified string is neither null nor empty, and includes the given message in the AssertionFailureException if the condition is violated.
    /// </summary>
    /// <param name="str">The string to assert.</param>
    /// <param name="message">A message to include in the exception if the condition is violated.</param>
    public static void NotNullOrEmpty(string str, string message)
    {
        if (String.IsNullOrEmpty(str))
        {
            throw new AssertionFailureException("Asserter.NotNullOrEmpty:" + message);
        }
    }

    /// <summary>
    /// Asserts that two objects are equal.
    /// </summary>
    /// <param name="obj1">The first object.</param>
    /// <param name="obj2">The second object.</param>
    public new static void Equals(object obj1, object obj2)
    {
        if (!(obj1.Equals(obj2)))
        {
            throw new AssertionFailureException("Asserter.Equals:Objects not equal");
        }
    }

    /// <summary>
    /// Asserts that two objects are equal, and includes the given message in the thrown exception if the condition is violated.
    /// </summary>
    /// <param name="obj1">The first object.</param>
    /// <param name="obj2">The second object.</param>
    /// <param name="message">A message to include in the thrown exception if the condition is violated.</param>
    public static void Equals(object obj1, object obj2, string message)
    {
        if (!(obj1.Equals(obj2)))
        {
            throw new AssertionFailureException("Asserter.Equals:" + message);
        }
    }

    /// <summary>
    /// Asserts that the two objects are equal by reference.
    /// </summary>
    /// <param name="obj1">The first object.</param>
    /// <param name="obj2">The second object.</param>
    public static void RefEquals(object obj1, object obj2)
    {
        if (obj1 != obj2)
        {
            throw new AssertionFailureException("Asserter.RefEquals:Objects not equal");
        }
    }

    /// <summary>
    /// Asserts that the two objects are equal by reference, and includes the given message in the thrown exception if the condition is violated.
    /// </summary>
    /// <param name="obj1">The first object.</param>
    /// <param name="obj2">The second object.</param>
    /// <param name="message">A message to include in the thrown exception if the condition is violated.</param>
    public static void RefEquals(object obj1, object obj2, string message)
    {
        if (obj1 != obj2)
        {
            throw new AssertionFailureException("Asserter.RefEquals:" + message);
        }
    }

    /// <summary>
    /// Asserts that a given GameObject contains a Component of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of Component the GameObject is expected to have.</typeparam>
    /// <param name="gameObject">The GameObject to assert.</param>
    public static void ContainsComponent<T>(GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>() == null)
        {
            throw new AssertionFailureException("Asserter.ContainsComponent<T>:GameObject does not contain specified Component");
        }
    }

    /// <summary>
    /// Asserts that a given GameObject contains a Component of the specified type, and includes the given message in the thrown exception if the condition is violated.
    /// </summary>
    /// <typeparam name="T">The type of Component the GameObject is expected to have.</typeparam>
    /// <param name="gameObject">The GameObject to assert.</param>
    /// <param name="message">A message to include in the thrown exception if the condition is violated.</param>
    public static void ContainsComponent<T>(GameObject gameObject, string message) where T : Component
    {
        if (gameObject.GetComponent<T>() == null)
        {
            throw new AssertionFailureException("Asserter.ContainsComponent<T>:" + message);
        }
    }

    /// <summary>
    /// Asserts that the given boolean value is true.
    /// </summary>
    /// <param name="value">The boolean value to assert.</param>
    public static void IsTrue(bool value)
    {
        if (!value)
        {
            throw new AssertionFailureException("Asserter.IsTrue:value is false");
        }
    }

    /// <summary>
    /// Asserts that the given boolean value is true, and includes the given message in the thrown exception if the condition is violated.
    /// </summary>
    /// <param name="value">The boolean value to assert.</param>
    /// <param name="message">A message to include in the thrown exception if the condition is violated.</param>
    public static void IsTrue(bool value, string message)
    {
        if (!value)
        {
            throw new AssertionFailureException("Asserter.IsTrue:" + message);
        }
    }
}

