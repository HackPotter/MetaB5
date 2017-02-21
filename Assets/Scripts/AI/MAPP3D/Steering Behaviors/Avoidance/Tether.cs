// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: Tether
	Behavior that allows the agent to remain within a tether space.
	
*/
[Serializable]
public class Tether : Steering
{
    [SerializeField]
    public Transform tetherPosition;

    [SerializeField]
    public float tetherRadius;

    /*
        Function: GetAcceleration
        Computes the acceleration need for the agent to avoid a collision with other agents
		
        Returns:
            a 3D vector representing the collision needed.
    */
    public override Vector3 GetAcceleration()
    {
        Vector3 acceleration = Vector3.zero;

        //get distance of agent from tether position
        Vector3 direction = tetherPosition.position - Agent.transform.position;
        float distance = direction.magnitude;

        //if agent is within tether radius, return zero acceleration.
        //otherwise give full acceleration towards the tether position
        if (distance < tetherRadius)
        {
            return acceleration;
        }

        acceleration = direction.normalized;
        acceleration *= Agent.MaxAcceleration;

        return acceleration;
    }

    /*
        Function: GetTetherPosition
        Get the center position of the tether space
		
        Returns:
            Vector3 object representing the center of the tether space
    */
    Vector3 GetTetherPosition()
    {
        return tetherPosition.position;
    }

    /*
        Function: GetTetherRadius
        Get the radius of the tether space 
		
        Returns:
            a float representing the radius of the tether space
    */
    public float GetTetherRadius()
    {
        return tetherRadius;
    }

    /*
        Function: SetTetherRadius
        Set the radius of the tether space
		
        Parameters:
            rad - the radius of the tether space
    */
    public void SetTetherRadius(float rad)
    {
        tetherRadius = rad;
    }
}
