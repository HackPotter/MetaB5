// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: Arrive
	Behavior that allows the agent to deccelerate towards the target.
	
	See Also:
		Seek, PursueArrive
*/
[Serializable]
public class Arrive : Steering
{
    [SerializeField]
    public Transform target;

    [SerializeField]
    public float targetRadius;

    [SerializeField]
    public float slowRadius;

    [SerializeField]
    public float timeToTarget;

    /*
        Function: Arrive
        Constructor
    */
    public Arrive()
    {
        targetRadius = 0.5f;
        slowRadius = 2.0f;
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
        if (target == null)
        {
            return Vector3.zero;
        }
        Vector3 acceleration = Vector3.zero;

        //get direction to target
        Vector3 direction = target.position - Agent.transform.position;
        float distance = direction.magnitude;

        //check if we are in target radius
        //if so, calculate the acceleration needed to stop
        if (distance < targetRadius)
        {
            acceleration = -Agent.Velocity / Time.deltaTime;
            return acceleration;
        }

        //check if we are outside the slow radius
        //if so, give full speed
        //if not, get scaled speed
        float targetSpeed;
        if (distance > slowRadius)
        {
            targetSpeed = Agent.MaxSpeed;
        }
        else
        {
            targetSpeed = Agent.MaxSpeed * distance / slowRadius;
        }

        //combine the target speed and direction
        Vector3 targetVelocity = direction.normalized * targetSpeed;

        //calculate acceleration
        acceleration = targetVelocity - Agent.Velocity;
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
