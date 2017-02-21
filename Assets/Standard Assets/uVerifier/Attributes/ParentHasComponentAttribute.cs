using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field,AllowMultiple=true)]
public class ParentHasComponentAttribute : ConditionVerifierAttribute
{
    public Type ComponentType
    {
        get;
        private set;
    }

    public ParentHasComponentAttribute(Type ComponentType)
    {
        this.ComponentType = ComponentType;
    }
}

