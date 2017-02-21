using System;

[AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
public class ExpressionAttribute : Attribute
{
    private bool _isDynamicType;
    private Type _evaluationType;
    private bool _showInEditor = true;

    public bool IsDynamicType
    {
        get { return _isDynamicType; }
        set { _isDynamicType = value; }
    }

    public Type EvaluationType
    {
        get { return _evaluationType; }
        set { _evaluationType = value; }
    }

    public bool ShowInEditor
    {
        get { return _showInEditor; }
        set { _showInEditor = value; }
    }

    public ExpressionAttribute()
    {
    }
}

