using UnityEngine;

[Expression(EvaluationType = typeof(int), IsDynamicType = false)]
public class IntLiteralExpression : LiteralExpression
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private int _value;
#pragma warning restore 0067, 0649

    public override object Value
    {
        get { return _value; }
    }
}
