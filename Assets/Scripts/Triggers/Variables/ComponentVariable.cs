using System;
using UnityEngine;

public class ComponentVariable : Variable
{
    [SerializeField]
    private Component _componentValue;

    public override object Value
    {
        get { return _componentValue; }
        set { _componentValue = value as Component; }
    }

    public override Type VariableType
    {
        get
        {
            return typeof(Component);
        }
    }
}
