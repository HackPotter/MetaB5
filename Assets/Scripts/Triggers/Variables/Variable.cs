using System;
using UnityEngine;

[Serializable]
public abstract class Variable : ScriptableObject
{
    public abstract object Value
    {
        get;
        set;
    }

    public abstract Type VariableType
    {
        get;
    }
}
