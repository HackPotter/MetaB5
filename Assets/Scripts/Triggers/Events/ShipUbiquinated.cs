using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Trigger(DisplayPath = "Biology")]
public class ShipUbiquinated : EventSender
{
    protected override void OnStart()
    {
        UbiquitinLigaseAgent.ShipUbiquinated += new Action(UbiquitinLigaseAgent_ShipUbiquinated);
    }

    void OnDestroy()
    {
        UbiquitinLigaseAgent.ShipUbiquinated -= UbiquitinLigaseAgent_ShipUbiquinated;
    }

    void UbiquitinLigaseAgent_ShipUbiquinated()
    {
        TriggerEvent();
    }
}

