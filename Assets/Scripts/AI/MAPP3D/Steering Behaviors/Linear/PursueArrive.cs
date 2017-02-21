// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

/*
	Class: PursueArrive
	Behavior that allows the agent to pursue and arrive at another agent.
	
	This is like Arrive except the agent predicts the future position of the target

	See Also:
		Arrive, Pursue
*/
public class PursueArrive : Pursue
{
    [SerializeField]
    protected float targetRadius;

    [SerializeField]
    protected float slowRadius;

    [SerializeField]
    protected float timeToTarget;

    /*
        Function: PursueArrive
        Constructor
    */
    public PursueArrive()
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
        Vector3 acceleration;

        //get distance to target
        float distance = (target.transform.position - Agent.transform.position).magnitude;

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
        acceleration = base.GetAcceleration();
        if (distance > slowRadius)
        {
            return acceleration;
        }
        else
        {
            Vector3 direction = acceleration.normalized;
            float targetSpeed = Agent.MaxSpeed * distance / slowRadius;
            Vector3 targetVelocity = direction * targetSpeed;
            acceleration = targetVelocity - Agent.Velocity;
            acceleration /= timeToTarget;
        }

        //clamp acceleration
        if (acceleration.magnitude > Agent.MaxAcceleration)
        {
            acceleration.Normalize();
            acceleration *= Agent.MaxAcceleration;
        }

        return acceleration;
    }
}
