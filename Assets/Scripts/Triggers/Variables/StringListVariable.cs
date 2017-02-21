using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class StringListVariable : Variable
{
    [SerializeField]
    private List<string> _stringList;

    public override object Value
    {
        get { return _stringList; }
        set { _stringList = value as List<string>; }
    }

    public override Type VariableType
    {
        get
        {
            return typeof(List<string>);
        }
    }
}

