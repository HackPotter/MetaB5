using System;
using System.Collections.Generic;
using UnityEngine;

/*
	Class: Separation
	Behavior that allows the agent to avoid collisions with other agents.  Use this for agents that move in unison with each other.
	
	See Also:
		CollisionAvoidance	
*/
[Serializable]
public class Separation : Steering
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private Radar _radar;
#pragma warning restore 0067, 0649
    
    /*
        Function: GetAcceleration
        Computes the acceleration need for the agent to avoid a collision with other agents
		
        Returns:
            a 3D vector representing the collision needed.
    */
    public override Vector3 GetAcceleration()
    {
        Vector3 acceleration = Vector3.zero;

        //get all targets
        List<Agent> neighbors = _radar.Neighbors;

        Vector3 steerAway = Vector3.zero;
        int count = 0;
        foreach (Agent agent in neighbors)
        {
            if (agent == this.Agent)
            {
                continue;
            }
            count++;
            steerAway += Agent.transform.position - agent.transform.position;
        }
        if (count == 0)
        {
            return Vector3.zero;
        }
        
        steerAway.Normalize();
        return steerAway;
    }
}