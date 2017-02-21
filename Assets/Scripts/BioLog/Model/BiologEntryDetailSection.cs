using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class BiologEntryDetailSection
{
    [SerializeField]
    private string _header;
    [SerializeField]
    private string _text;

    public string Header
    {
        get { return _header; }
        set { _header = value; }
    }

    public string Text
    {
        get { return _text; }
        set { _text = value; }
    }

    public BiologEntryDetailSection()
    {
        if (_header == null)
        {
            _header = "";
        }
        if (_text == null)
        {
            _text = "";
        }
    }
}
