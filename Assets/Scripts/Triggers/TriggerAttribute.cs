using System;

[AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
public class TriggerAttribute : Attribute
{
    public string DisplayPath
    {
        get;
        set;
    }

    public string UserFriendlySegment
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public TriggerAttribute()
    {
    }
}

