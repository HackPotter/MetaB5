// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;
/*
	Class: Seek
	Behavior that allows the agent to seek a target Transform game object.

	See Also:
		Arrive, Pursue
*/
[Serializable]
public class Seek : Steering
{
    [SerializeField]
    public Transform target;

    /*
        Function: Seek
        Constructor
    */
    public Seek()
    {
    }

    /*
        Function: GetAcceleration
        Computes the acceleration need for the agent to avoid a collision with other agents
		
        Returns:
            a 3D vector representing the collision needed.
    */
    public override Vector3 GetAcceleration()
    {
        //Debug.Log(target.position.ToString());
        Vector3 acceleration = target.position - Agent.transform.position;

        acceleration.Normalize();
        //acceleration *= Agent.MaxAcceleration;

        return acceleration;
    }

    /*
        Function: SetTarget
        Sets the target transform for the agent to seek
		
        Parameters:
            target - the target transform
    */
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
