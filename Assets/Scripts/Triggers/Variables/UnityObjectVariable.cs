using System;
using UnityEngine;

public class UnityObjectVariable : Variable
{
    [SerializeField]
    private UnityEngine.Object _objectValue;

    public override object Value
    {
        get { return _objectValue; }
        set { _objectValue = value as UnityEngine.Object; }
    }

    public override Type VariableType
    {
        get
        {
            return typeof(UnityEngine.Object);
        }
    }

}
