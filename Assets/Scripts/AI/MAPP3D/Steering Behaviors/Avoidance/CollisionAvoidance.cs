// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: CollisionAvoidance
	Behavior that allows the agent to avoid collisions with other agents.  Use this for agents that do not move in unison (flock) with each other.
	
	See Also:
		ObstacleAvoidance
*/
[Serializable]
public class CollisionAvoidance : Steering
{
    [SerializeField]
    protected float radius;

    [SerializeField]
    protected string avoidTag;

    /*
        Function: CollisionAvoidance
        Constructor
    */
    public CollisionAvoidance()
    {
        radius = 1;
    }

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
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(avoidTag);
        Agent[] targets = new Agent[targetObjects.Length - 1];
        int iter = 0;
        int goIter = 0;

        while (goIter < targetObjects.Length && iter < targets.Length)
        {
            if (targetObjects[goIter] != Agent.gameObject)
            {
                targets[iter] = targetObjects[goIter].GetComponent<Agent>();
                iter++;
            }
            goIter++;
        }

        //store the first collision time
        float shortestTime = Mathf.Infinity;

        //store data that we will need and can avoid recalculating
        Agent firstTarget = null;
        float firstMinSeparation = 0;
        float firstDistance = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            //calculate time to collision
            Vector3 relativePos = targets[i].transform.position - Agent.transform.position;
            Vector3 relativeVel = targets[i].Velocity - Agent.Velocity;
            float relativeSpeed = relativeVel.magnitude;
            if (relativeSpeed == 0)
            {
                continue;
            }
            float timeToCollision = -Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
            Vector3 minSeparation = relativePos + timeToCollision * relativeVel;
            if (minSeparation.magnitude > 2 * radius)
            {
                continue;
            }

            //check if its the shortest
            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                //store data
                shortestTime = timeToCollision;
                firstTarget = targets[i];
                firstMinSeparation = minSeparation.magnitude;
                firstDistance = relativePos.magnitude;
            }

        }//end loop

        //if we have no target, exit
        if (firstTarget == null)
        {
            return acceleration;
        }

        //if we are already colliding, steer based on current position
        if (firstMinSeparation <= 0 || firstDistance < 2 * radius)
        {
            acceleration = Agent.transform.position - firstTarget.transform.position;
        }
        else
        {
            acceleration = (Agent.transform.position - firstTarget.transform.position) + shortestTime * (Agent.Velocity - firstTarget.Velocity);
        }

        //avoid the target
        acceleration.Normalize();

        return acceleration;


    }

    /*
        Function:SetRadius
        Sets the radius average radius of the agents to avoid.  A good value is 0.7f unless the scale of the agents are not equal to 1.
		
        Parameters:
            radius - the average radius of the agents
    */
    public void SetRadius(float radius)
    {
        this.radius = radius;
    }

    /*
        Function: SetTag
        Sets the tag that identifies the agents to avoid. Agents that need to be avoided must have this tag
		
        Parameters:
            tag - the tag of the agents
    */
    public void SetTag(string tag)
    {
        this.avoidTag = tag;
    }
}