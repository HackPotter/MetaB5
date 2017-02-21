using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IContextMessageView
{
    void SetLowPriorityText(string text);
    void SetText(string text);
    void SetText(string text, Color color);

}

