using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Trigger(DisplayPath = "Biology")]
public class ShipFullyUbiquinated : EventSender
{
    protected override void OnStart()
    {
        Substrate.ShipFullyUbiquintated += new Action(UbiquitinLigaseAgent_ShipUbiquinated);
    }

    void OnDestroy()
    {
        Substrate.ShipFullyUbiquintated -= UbiquitinLigaseAgent_ShipUbiquinated;
    }

    void UbiquitinLigaseAgent_ShipUbiquinated()
    {
        TriggerEvent();
    }
}

