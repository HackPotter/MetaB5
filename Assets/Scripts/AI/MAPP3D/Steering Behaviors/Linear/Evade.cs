// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: Evade
	Behavior that allows the agent to evade a target agent by predicting the future position of the target.
	
	See Also:
		Flee
*/
[Serializable]
public class Evade : Steering
{
    [SerializeField]
    public Agent target;

    [SerializeField]
    public float maxPrediction;

    /*
        Function: Evade
        Constructor
    */
    public Evade()
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
        //get direction to target
        Vector3 direction = Agent.transform.position - target.transform.position;
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
        Vector3 predictedTargetPosition = target.transform.position + target.GetComponent<Rigidbody>().velocity * prediction;

        //calculate direction to predicted target and give full acceleration
        Vector3 acceleration = Agent.transform.position - predictedTargetPosition;
        acceleration.Normalize();
        acceleration *= Agent.MaxAcceleration;

        return acceleration;
    }
}
