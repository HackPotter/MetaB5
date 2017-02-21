using UnityEngine;

[Trigger(DisplayPath = "Data")]
public class SetVariableValue : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(Variable),"Variable to Set")]
    private Expression _variableToSet;

    [SerializeField]
    [ExpressionField(typeof(object),"Value to Assign")]
    private Expression _value;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        Variable variable = context.Evaluate<Variable>(_variableToSet);
        variable.Value = context.Evaluate<object>(_value);
    }
}

