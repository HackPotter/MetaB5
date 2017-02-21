using UnityEngine;

[Expression(IsDynamicType=true)]
public class VariableExpression : Expression
{
    [SerializeField]
    private string _variableIdentifier;

    public string VariableIdentifier
    {
        get { return _variableIdentifier; }
        set { _variableIdentifier = value; }
    }
}

