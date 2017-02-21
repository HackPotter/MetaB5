using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class DialogueNodeData
{
    [SerializeField]
    private string _sender = "";
    [SerializeField]
    private string _message = "";

    public string Sender
    {
        get { return _sender; }
        set { _sender = value; }
    }

    public string Message
    {
        get { return _message; }
        set { _message = value; }
    }
}

