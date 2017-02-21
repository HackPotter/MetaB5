// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using System;
using UnityEngine;

/*
    Class: Steering
    Base class for linear and avoidance behaviors

*/
[RequireComponent(typeof(AgentBehavior))]
public abstract class Steering : MonoBehaviour
{
    [SerializeField]
    private float _weight;

    public float Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public Agent Agent
    {
        get;
        set;
    }

    void OnEnable()
    {
        GetComponent<AgentBehavior>().RegisterSteering(this);
    }

    void OnDisable()
    {
        GetComponent<AgentBehavior>().DeregisterSteering(this);
    }

    public abstract Vector3 GetAcceleration();
}
