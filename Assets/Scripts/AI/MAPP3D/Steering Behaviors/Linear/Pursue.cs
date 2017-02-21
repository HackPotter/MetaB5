// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: Pursue
	Behavior that allows the agent to pursue another agent.
	
	This is like seek except the agent predicts the future position of the target

	See Also:
		Seek, PursueArrive
*/
[Serializable]
public class Pursue : Steering
{
    [SerializeField]
    public Agent target;

    [SerializeField]
    public float maxPrediction;

    /*
        Function: Pursue
        Constructor
    */
    public Pursue()
    {
        maxPrediction = 4;
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
        //get direction to target
        Vector3 direction = target.transform.position - Agent.transform.position;
        float distance = direction.magnitude;

        //get current speed of the agent
        float speed = Agent.Velocity.magnitude;

        //check if speed is too small to give reasonable prediction
        float prediction;
        if (speed <= distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        //get predicted target position
        Vector3 predictedTargetPosition = target.transform.position + target.Velocity * prediction;

        //calculate direction to predicted target and give full acceleration
        Vector3 acceleration = predictedTargetPosition - Agent.transform.position;
        acceleration.Normalize();
        acceleration *= Agent.MaxAcceleration;

        return acceleration;
    }

    /*
        Function: GetTarget
        Gets the target agent
		
        Returns:
            the target agent
    */
    public Agent GetTarget()
    {
        return target;
    }

    /*
        Function: SetTarget
        Sets the target agent
		
        Parameters:
            target - the target agent
    */
    public void SetTarget(Agent target)
    {
        this.target = target;
    }

    /*
        Function: SetMaxPrediction
        Sets the maximum prediction time
		
        Parameters:
            maxPrediction - the maximum prediction time
    */
    public void SetMaxPrediction(float maxPrediction)
    {
        this.maxPrediction = maxPrediction;
    }
}