using System;

public abstract class LiteralExpression : Expression
{
    public abstract object Value
    {
        get;
    }
}

