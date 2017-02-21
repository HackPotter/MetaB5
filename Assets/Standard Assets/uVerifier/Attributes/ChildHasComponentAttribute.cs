using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field ,AllowMultiple=true)]
public class ChildHasComponentAttribute : ConditionVerifierAttribute
{
    public Type ComponentType
    {
        get;
        private set;
    }

    public ChildHasComponentAttribute(Type ComponentType)
    {
        this.ComponentType = ComponentType;
    }
}

