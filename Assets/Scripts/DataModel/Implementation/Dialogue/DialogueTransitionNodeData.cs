using System;
using UnityEngine;


[Serializable]
public class DialogueTransitionNodeData
{
    [SerializeField]
    private string _transitionText;

    public string TransitionText
    {
        get { return _transitionText; }
        set { _transitionText = value; }
    }
}

