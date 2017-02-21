using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


// Add ExecuteInEditoMode to EventSender.

// In Awake, check if editor mode. If editor mode, add required EventOutputArgument components if necessary.

public class EventOutputArgument : DataProvider
{
    public override object GetValue()
    {
        return null;
    }
}

