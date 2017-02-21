using UnityEngine;

[Trigger(DisplayPath = "Variables/Are Strings Equal",
    Description = "Compares two strings and invokes the actions underneath if they are equal.")]
public class AreStringsEqual : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(string), "string 1")]
    private Expression _string1;

    [SerializeField]
    [ExpressionField(typeof(string), "string 2")]
    private Expression _string2;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_string1 == null || _string2 == null)
        {
            DebugFormatter.LogError(this, "Expecting string, but Expression is null.");
            return;
        }

        string string1 = context.Evaluate<string>(_string1);
        string string2 = context.Evaluate<string>(_string2);

        if (string1 == string2)
        {
            TriggerEvent(context);
        }
    }
}

