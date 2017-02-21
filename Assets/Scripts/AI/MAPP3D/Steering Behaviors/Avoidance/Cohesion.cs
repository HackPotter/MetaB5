using System.Collections.Generic;
using UnityEngine;


// Cohesion
//      Find average position of neighbors
//      Steer toward that position.
public class Cohesion : Steering
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private Radar _radar;
#pragma warning restore 0067, 0649

    public override Vector3 GetAcceleration()
    {
        List<Agent> neighbors = _radar.Neighbors;

        Vector3 averageNeighborPosition = Vector3.zero;
        int count = 0;
        foreach (Agent neighbor in neighbors)
        {
            count++;
            averageNeighborPosition += neighbor.transform.position;
        }

        if (count == 0)
        {
            return Vector3.zero;
        }

        averageNeighborPosition /= count;

        return (averageNeighborPosition - Agent.transform.position).normalized;
    }
}
