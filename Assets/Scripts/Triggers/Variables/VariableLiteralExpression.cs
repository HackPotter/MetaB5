using UnityEngine;

[Expression(EvaluationType = typeof(Variable), IsDynamicType = false, ShowInEditor=false)]
public class VariableLiteralExpression : LiteralExpression
{
    [SerializeField]
    private Variable _value;

    public Variable VariableValue
    {
        get { return _value; }
        set { _value = value; }
    }

    public override object Value
    {
        get { return _value; }
    }
}