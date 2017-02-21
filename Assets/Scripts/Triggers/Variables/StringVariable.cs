using UnityEngine;

public class StringVariable : Variable
{
    [SerializeField]
    private string _value;

    public string StringValue
    {
        get { return _value; }
        set { _value = value; }
    }

    public override object Value
    {
        get { return StringValue; }
        set { _value = value as string; }
    }

    public override System.Type VariableType
    {
        get { return typeof(string); }
    }
}