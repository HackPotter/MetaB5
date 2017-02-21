// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

// Instead of Agent, use Vehicle?

[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour
{
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _maxAcceleration;

    [SerializeField]
    private AgentBehavior _activeBehavior;

    public float MaxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = value; }
    }

    public float MaxAcceleration
    {
        get { return _maxAcceleration; }
        set { _maxAcceleration = value; }
    }

    public float MaxAngularSpeed
    {
        get { return GetComponent<Rigidbody>().maxAngularVelocity; }
    }

    public Vector3 Velocity
    {
        get { return GetComponent<Rigidbody>().velocity; }
    }

    public AgentBehavior ActiveBehavior
    {
        get { return _activeBehavior; }
        set { _activeBehavior = value; }
    }

    protected virtual void FixedUpdate()
    {
		if (GameState.Instance.PauseLevel == PauseLevel.Unpaused)
		{
        	if (ActiveBehavior != null && !GetComponent<Rigidbody>().isKinematic)
        	{
	            Vector3 acceleration = ActiveBehavior.GetAcceleration() * _maxAcceleration;
            	//if (acceleration.magnitude > _maxAcceleration)
            	//{
            	//    acceleration = acceleration.normalized * _maxAcceleration;
            	//}
	
            	GetComponent<Rigidbody>().AddForce(acceleration, ForceMode.Acceleration);
            	if (GetComponent<Rigidbody>().velocity.magnitude > _maxSpeed)
            	{
	                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * _maxSpeed;
            	}
        	}
		}
    }
}