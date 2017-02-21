using System;
using UnityEngine;

[Serializable]
public class IntVariable : Variable
{
    [SerializeField]
    private int _value;

    public int IntValue
    {
        get { return _value; }
        set { _value = value; }
    }

    public override object Value
    {
        get { return IntValue; }
        set { _value = (int)value; }
    }

    public override Type VariableType
    {
        get { return typeof(int); }
    }
}

