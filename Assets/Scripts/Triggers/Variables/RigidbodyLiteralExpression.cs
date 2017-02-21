using UnityEngine;

[Expression(EvaluationType = typeof(Rigidbody), IsDynamicType = false)]
public class RigidbodyLiteralExpression : LiteralExpression
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private Rigidbody _value;
#pragma warning restore 0067, 0649

    public override object Value
    {
        get { return _value; }
    }
}
