using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RigidbodyVariable : Variable
{
    [SerializeField]
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody
    {
        get { return _rigidbody; }
        set { _rigidbody = value; }
    }

    public override object Value
    {
        get
        {
            return _rigidbody;
        }
        set
        {
            _rigidbody = (Rigidbody)value;
        }
    }

    public override Type VariableType
    {
        get { return typeof(Rigidbody); }
    }
}

