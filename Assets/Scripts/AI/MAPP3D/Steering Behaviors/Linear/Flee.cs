using System;
using UnityEngine;

/*
	Class: Flee
	Behavior that allows the agent to evade another agent.
	
	See Also:
		Evade
*/
public class Flee : Steering
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float _distanceThreshold;
#pragma warning restore 0067, 0649


    /*
        Function: GetAcceleration
        Computes the acceleration need for the agent to avoid a collision with other agents
		
        Returns:
            a 3D vector representing the collision needed.
    */
    public override Vector3 GetAcceleration()
    {
        if (Vector3.Distance(Agent.transform.position, target.position) < _distanceThreshold)
        {
            Vector3 acceleration = Agent.transform.position - target.transform.position;
            acceleration.Normalize();
            return acceleration;
        }

        return Vector3.zero;
    }
}
