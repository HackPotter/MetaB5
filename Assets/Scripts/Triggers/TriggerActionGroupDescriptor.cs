using System;
using UnityEngine;

[Serializable]
public class TriggerActionGroupDescriptor
{
    [SerializeField]
    //[HideInInspector]
    private string _actionGroupName;
    
    [SerializeField]
    [HideInInspector]
    private string _actionGroupDescription;

    public string ActionGroupName
    {
        get { return _actionGroupName; }
    }

    public string ActionGroupDescription
    {
        get { return _actionGroupDescription; }
    }

    public TriggerActionGroupDescriptor(string name, string description)
    {
        _actionGroupName = name;
        _actionGroupDescription = description;
    }
}

