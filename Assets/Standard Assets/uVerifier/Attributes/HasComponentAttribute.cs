using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Field,AllowMultiple=true)]
public class HasComponentAttribute : ConditionVerifierAttribute
{
    public Type ComponentType
    {
        get;
        private set;
    }

    public HasComponentAttribute(Type ComponentType)
    {
        this.ComponentType = ComponentType;
    }
}
