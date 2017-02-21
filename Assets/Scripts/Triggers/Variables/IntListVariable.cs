using System;
using System.Collections.Generic;
using UnityEngine;

public class IntListVariable : Variable
{
    [SerializeField]
    private List<int> _intList;

    public override object Value
    {
        get
        {
            return _intList;
        }
        set
        {
            _intList = (List<int>)value;
        }
    }

    public override Type VariableType
    {
        get
        {
            return typeof(List<int>);
        }
    }
}

