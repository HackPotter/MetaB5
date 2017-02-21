// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: SeekPoint
	Behavior that allows the agent to seek a 3D point.

	See Also:
		Seek
*/
[Serializable]
public class SeekPoint : Steering
{
    protected Vector3 point;

    /*
        Function: SeekPoint
        Constructor
    */
    public SeekPoint()
    {
        point = Vector3.zero;
    }

    /*
        Function: SetPoint
        Sets the point to seek
		
        Parameters:
            point - the point to seek
    */
    public void SetPoint(Vector3 point)
    {
        this.point = point;
    }

    /*
        Function: GetAcceleration
        Computes the acceleration need for the agent to avoid a collision with other agents
		
        Returns:
            a 3D vector representing the collision needed.
    */
    public override Vector3 GetAcceleration()
    {
        Vector3 acceleration = point - Agent.transform.position;

        acceleration.Normalize();
        acceleration *= Agent.MaxAcceleration;

        return acceleration;
    }
}
