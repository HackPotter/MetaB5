using System;

public class DynamicVariable : Variable
{
    private Type _type;
    private object _value;

    public override object Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
        }
    }

    public override Type VariableType
    {
        get { return _type; }
    }

    public void SetDynamicType(Type type)
    {
        _type = type;
    }
}
