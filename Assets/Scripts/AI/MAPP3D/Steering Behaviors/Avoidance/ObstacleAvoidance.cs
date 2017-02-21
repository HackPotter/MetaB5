// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: ObstacleAvoidance
	Behavior that allows the agent to avoid collisions with static (non-moving) objects
*/
[Serializable]
public class ObstacleAvoidance : Steering
{
    [SerializeField]
    protected float lookAhead;

    [SerializeField]
    protected float avoidDistance;

    [SerializeField]
    protected string layer;

    /*
        Function: ObstacleAvoidance
        Constructor
    */
    public ObstacleAvoidance()
    {
        avoidDistance = 1.0f;
        lookAhead = 3.0f;
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
        //LayerMask layerMask = 1 << 8;//fix: must be dynamic
        LayerMask layerMask = 1 << LayerMask.NameToLayer(layer);

        //get hit info
        CapsuleCollider collider = Agent.GetComponent<CapsuleCollider>();
        if (!collider)
        {
            Debug.LogError("The agent must have a capsule collider attached!!!\nDestroying agent...");
            UnityEngine.Object.Destroy(Agent);
            return acceleration;
        }

        RaycastHit hit;
        Vector3 p1 = Agent.transform.TransformPoint(collider.center);
        Vector3 p2 = Agent.transform.TransformPoint(collider.center);
        p1[collider.direction] -= collider.height / 2.0f;
        p2[collider.direction] += collider.height / 2.0f;
        bool isHit = Physics.CapsuleCast(p1, p2, collider.radius, Agent.Velocity, out hit, lookAhead, layerMask);//fix:add a layer to ignore

        if (!isHit)
        {
            return acceleration;
        }
        else
        {
            //seek a point in space away from obstacle
            Debug.DrawRay(Agent.transform.position, Agent.Velocity.normalized * lookAhead, Color.red);
            Debug.DrawRay(hit.point, hit.normal * avoidDistance, Color.yellow);
        }

        //get direction to target and give full acceleration
        acceleration = -Agent.transform.position;
        acceleration.Normalize();
        acceleration *= Agent.MaxAcceleration;

        return acceleration;
    }

    /*
        Function: SetLayer
        Sets the layer that contains the obstacles.  Game objects to avoid must be in this layer!
		
        Any game objects not in this layer will not be checked!
		
        Parameters:
            layer - the name of the layer that contains the objects to be avoided.
    */
    public void SetLayer(string layer)
    {
        this.layer = layer;
    }

    /*
        Function: SetLookAhead
        Sets how far ahead the agent should "look" for future collisions
		
        Parameters:
            lookAhead - how far ahead will the agent consider collisions
    */
    public void SetLookAhead(float lookAhead)
    {
        this.lookAhead = lookAhead;
    }

    /*
        Function: SetAvoidDistance
        Sets how close the agent can get to objects.  A good value is 0.7f for agents at a scale of 1.
		
        Parameters:
            avoidDistance - how close can the agent get to obstacles.
    */
    public void SetAvoidDistance(float avoidDistance)
    {
        this.avoidDistance = avoidDistance;
    }

}