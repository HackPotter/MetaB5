using UnityEngine;
using System.Collections;

public class QualityBehaviours : MonoBehaviour {

    [SerializeField]
    public Behaviour[] LowSettings;

    [SerializeField]
    public Behaviour[] MediumSettings;

    [SerializeField]
    public Behaviour[] HighSettings;

    
    public void SetLow()
    {
        foreach (Behaviour b in MediumSettings)
        {
            b.enabled = false;
        }

        foreach (Behaviour b in HighSettings)
        {
            b.enabled = false;
        }

        foreach (Behaviour b in LowSettings)
        {
            b.enabled = true;
        }
    }

    public void SetMedium()
    {
        foreach (Behaviour b in LowSettings)
        {
            b.enabled = false;
        }
        
        foreach (Behaviour b in HighSettings)
        {
            b.enabled = false;
        }

        foreach (Behaviour b in MediumSettings)
        {
            b.enabled = true;
        }
    }

    public void SetHigh()
    {
        foreach (Behaviour b in LowSettings)
        {
            b.enabled = false;
        }

        foreach (Behaviour b in MediumSettings)
        {
            b.enabled = false;
        }

        foreach (Behaviour b in HighSettings)
        {
            b.enabled = true;
        }
    }
}
