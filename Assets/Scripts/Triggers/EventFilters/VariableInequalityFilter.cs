using UnityEngine;

[Trigger(Description = "Compares the value of two variables. If they are equal, the actions under this filter are invoked.")]
public class VariableInequalityFilter : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(object), "Variable 1")]
    private Expression _variable1;

    [SerializeField]
    [ExpressionField(typeof(object), "Variable 2")]
    private Expression _variable2;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        object value1 = context.Evaluate<object>(_variable1);
        object value2 = context.Evaluate<object>(_variable2);

        if (!value1.Equals(value2))
        {
            TriggerEvent(context);
        }
    }
}

