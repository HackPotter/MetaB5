using System;

[AttributeUsage(AttributeTargets.Field,AllowMultiple=false)]
public class ExpressionFieldAttribute : Attribute
{
    private Type _expressionType;
    private string _displayName;

    public Type ExpressionType
    {
        get { return _expressionType; }
        set { _expressionType = value; }
    }

    public string DisplayName
    {
        get { return _displayName; }
        set { _displayName = value; }
    }

    public ExpressionFieldAttribute(Type expressionType, string displayName)
    {
        _expressionType = expressionType;
        _displayName = displayName;
    }
}

