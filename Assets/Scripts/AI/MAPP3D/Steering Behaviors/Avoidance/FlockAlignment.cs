using System.Collections.Generic;
using UnityEngine;

public class FlockAlignment : Steering
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private Radar _radar;
#pragma warning restore 0067, 0649

    public override Vector3 GetAcceleration()
    {
        List<Agent> neighbors = _radar.Neighbors;

        Vector3 averageHeading = Vector3.zero;
        int count = 0;
        foreach (Agent neighbor in neighbors)
        {
            if (neighbor == Agent)
            {
                continue;
            }
            count++;
            averageHeading += neighbor.Velocity;
        }

        if (count == 0)
        {
            return Vector3.zero;
        }

        averageHeading /= count;
        averageHeading.Normalize();

        return averageHeading;
    }
}

