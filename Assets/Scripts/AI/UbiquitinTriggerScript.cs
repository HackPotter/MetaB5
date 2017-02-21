using UnityEngine;

public class UbiquitinTriggerScript : MonoBehaviour
{
    private bool shipDetected = false;

    void OnTriggerEnter(Collider enteredCollider)
    {
        if (!enteredCollider.name.Equals("Ship"))
        {
            return;
        }

        shipDetected = true;
    }

    void OnTriggerExit(Collider exitedCollider)
    {
        if (!exitedCollider.name.Equals("Ship"))
        {
            return;
        }

        shipDetected = false;
    }

    public bool IsShipDetected()
    {
        return shipDetected;
    }
}