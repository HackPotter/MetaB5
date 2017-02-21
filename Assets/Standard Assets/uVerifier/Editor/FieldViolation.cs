using UnityEngine;

/// <summary>
/// FieldViolation contains information about a GameObject that has violated a condition.
/// </summary>
public class FieldViolation
{
    /// <summary>
    /// Message containing details about the violation.
    /// </summary>
    public string Message
    {
        get;
        private set;
    }

    /// <summary>
    /// The GameObject on which the violation occurs.
    /// </summary>
    public GameObject ViolatingObject
    {
        get;
        private set;
    }

    /// <summary>
    /// The object causing the violation on the GameObject. May be null.
    /// </summary>
    public object Target
    {
        get;
        private set;
    }

    /// <summary>
    /// Constructs a FieldViolation object.
    /// </summary>
    /// <param name="ViolatingObject">The GameObject on which the violation occurs.</param>
    /// <param name="Target">The object causing the violation on the GameObject. May be null.</param>
    /// <param name="Message">A message containing details about the violation.</param>
    public FieldViolation(GameObject ViolatingObject, object Target, string Message)
    {
        Asserter.NotNull(ViolatingObject);
        Asserter.NotNullOrEmpty(Message);

        this.ViolatingObject = ViolatingObject;
        this.Target = Target;
        this.Message = Message;
    }
}