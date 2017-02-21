// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class Align : MonoBehaviour
{

    /*
        Class: Align
        an angular behavior that allows the agent to match its alignment with the target
	
        All UnityScripts are automatically derived from MonoBehaviour, even though a class is not
        defined as with other agent scripts ie Wanderer, Evader etc.
    */

    public Transform target;

    void FixedUpdate()
    {
        //this helps the agent match its alignment with the target
        //ie., if the target's z-direction is (0, 0, 1) then the agent's z-direction will be (0, 0, 1)

        Agent agent = transform.GetComponent<Agent>();

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, agent.MaxAngularSpeed);
    }
}