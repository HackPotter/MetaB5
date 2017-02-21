// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class Face : MonoBehaviour
{

    /*
        Class: Face
        an angular behavior that allows the agent to match its alignment to the direction of the target
	
        All UnityScripts are automatically derived from MonoBehaviour, even though a class is not
        defined as with other agent scripts ie Wanderer, Evader etc.
    */



    public Transform target;

    void FixedUpdate()
    {
        //this helps the agent face the direction of the target
        //ie., if the target's is directly to the right of the agent then the agent's forward direciton will be (1, 0, 0);

        Agent agent = transform.GetComponent<Agent>();

        Vector3 direction = target.position - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.MaxAngularSpeed);
        }
    }
}