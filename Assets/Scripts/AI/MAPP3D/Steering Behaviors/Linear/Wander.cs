// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: Wander
	Behavior that allows the agent to wander UnityEngine.Randomly. Best when coupled with tether behavior.

	See Also:
		Tether
*/
[Serializable]
public class Wander : Steering
{
    [SerializeField]
    public float wanderRadiusXZ = 4;

    [SerializeField]
    public float wanderRadiusY = 2;

    [SerializeField]
    public float wanderRate = 0.2f;

    [SerializeField]
    public float wanderOffset = 5;

    [SerializeField]
    public Vector3 wanderDirection;

    /*
        Function: GetAcceleration
        Computes the acceleration need for the agent to avoid a collision with other agents
		
        Returns:
            a 3D vector representing the collision needed.
    */
    public override Vector3 GetAcceleration()
    {
        Vector3 acceleration;

        //update wander direction
        wanderDirection.x += (UnityEngine.Random.Range(-1.0f, 1.0f) - UnityEngine.Random.Range(-1.0f, 1.0f)) * wanderRate;
        wanderDirection.y += (UnityEngine.Random.Range(-1.0f, 1.0f) - UnityEngine.Random.Range(-1.0f, 1.0f)) * wanderRate;
        wanderDirection.z += (UnityEngine.Random.Range(-1.0f, 1.0f) - UnityEngine.Random.Range(-1.0f, 1.0f)) * wanderRate;
        wanderDirection.Normalize();


        //calculate target point on imaginary sphere
        Vector3 point = Agent.transform.position;
        point.x += wanderDirection.x * wanderRadiusXZ;
        point.y += wanderDirection.y * wanderRadiusY;
        point.z += wanderDirection.z * wanderRadiusXZ;

        //move imaginary sphere in front of agent
        point += Agent.transform.forward * wanderOffset;
        //Debug.DrawLine(agent.transform.position, point, Color.yellow);

        //get direction from point on shpere to agent
        Vector3 targetDirection = point - Agent.transform.position;

        //give full acceleration in targetDirection
        targetDirection.Normalize();
        acceleration = targetDirection * Agent.MaxAcceleration;

        return acceleration;
    }

    /*
        Function: SetWanderRate
        Sets the wander rate
		
        Parameters:
            wanderRate - a float representing the wander rate
    */
    void SetWanderRate(float wanderRate)
    {
        this.wanderRate = wanderRate;
    }
}
