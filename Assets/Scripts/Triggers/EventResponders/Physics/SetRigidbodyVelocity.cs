using UnityEngine;

[Trigger(Description = "Sets the velocity of the given rigidbody.", DisplayPath = "Physics")]

public class SetRigidbodyVelocity : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The velocity of the rigidbody.")]
    private Vector3 _velocity;

    [SerializeField]
    [Infobox("The angular velocity of the rigidbody.")]
    private Vector3 _angularVelocity;

    [SerializeField]
    [ExpressionField(typeof(Rigidbody), "Rigidbody")]
    private Expression _rigidbody;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        Rigidbody rigidbody = context.Evaluate<Rigidbody>(_rigidbody);
        rigidbody.velocity = _velocity;
        rigidbody.angularVelocity = _angularVelocity;
    }
}

