using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BiologData : ScriptableObject
{
    [SerializeField]
    private List<BiologEntry> _entries;

    public List<BiologEntry> Entries
    {
        get { return _entries; }
    }

    void OnEnable()
    {
        if (_entries == null)
        {
            _entries = new List<BiologEntry>();
        }
    }
}