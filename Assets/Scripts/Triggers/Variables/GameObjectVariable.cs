using System;
using UnityEngine;

[Serializable]
public class GameObjectVariable : Variable
{
    [SerializeField]
    private GameObject _value;

    public GameObject GameObjectValue
    {
        get { return _value; }
        set { _value = value; }
    }

    public override object Value
    {
        get { return GameObjectValue; }
        set { _value = value as GameObject; }
    }

    public override Type VariableType
    {
        get { return typeof(GameObject); }
    }
}

