using UnityEngine;

[Expression(EvaluationType=typeof(string),IsDynamicType=false)]
public class StringLiteralExpression : LiteralExpression
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _value;
#pragma warning restore 0067, 0649

    public override object Value
    {
        get { return _value; }
    }
}

