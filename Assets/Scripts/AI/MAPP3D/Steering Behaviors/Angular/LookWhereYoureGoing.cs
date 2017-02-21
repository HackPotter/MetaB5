// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class LookWhereYoureGoing : MonoBehaviour
{
    /*
        Class: Face
        an angular behavior that allows the agent to match its alignment to the direction of the target
	
        All UnityScripts are automatically derived from MonoBehaviour, even though a class is not
        defined as with other agent scripts ie Wanderer, Evader etc.
    */


    private Agent _agent;

    void Awake()
    {
        _agent = GetComponent<Agent>();
    }

    void FixedUpdate()
    {

        //this helps the agent face the direction it is going
        //ie., if the agent is moving along the forward axis in world space then the agent will face (0, 0, 1)

        if (_agent.Velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_agent.Velocity, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _agent.MaxAngularSpeed);
        }
    }
}