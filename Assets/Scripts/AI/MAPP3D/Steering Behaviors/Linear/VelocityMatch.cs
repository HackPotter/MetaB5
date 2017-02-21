// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: VelocityMatch
	Behavior that allows the agent to match its velocity with a target agent
*/
[Serializable]
public class VelocityMatch : Steering
{
    [SerializeField]
    protected Agent target;

    [SerializeField]
    protected float timeToTarget;


    /*
        Function: VelocityMatch
        Constructor
    */
    public VelocityMatch()
    {
        timeToTarget = 0.1f;
    }

    /*
        Function: GetAcceleration
        Computes the acceleration need for the agent to avoid a collision with other agents
		
        Returns:
            a 3D vector representing the collision needed.
    */
    public override Vector3 GetAcceleration()
    {
        Vector3 acceleration;

        //compute acceleration to the target's velocity
        acceleration = target.Velocity - Agent.Velocity;
        acceleration /= timeToTarget;

        //clamp acceleration
        if (acceleration.magnitude > Agent.MaxAcceleration)
        {
            acceleration.Normalize();
            acceleration *= Agent.MaxAcceleration;
        }

        return acceleration;
    }
}
