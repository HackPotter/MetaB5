using System;
using UnityEngine;

public class InfoboxAttribute : PropertyAttribute
{
    public string Message
    {
        get;
        private set;
    }

    public InfoboxAttribute(string message)
    {
        Message = message;
    }
}
