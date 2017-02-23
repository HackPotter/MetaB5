using System.Collections.Generic;
using UnityEngine;

public class AgentBehavior : MonoBehaviour {
#pragma warning disable 0067, 0649
    [SerializeField]
    private Agent _agent;
#pragma warning restore 0067, 0649

    private List<Steering> _allSteerings = new List<Steering>();

    public Vector3 GetAcceleration() {
        Vector3 acceleration = Vector3.zero;
        foreach (Steering steering in _allSteerings) {
            acceleration += steering.GetAcceleration() * steering.Weight / _allSteerings.Count;
        }
        return acceleration;
    }

    public void RegisterSteering(Steering steering) {
        _allSteerings.Add(steering);
        steering.Agent = _agent;
    }

    public void DeregisterSteering(Steering steering) {
        _allSteerings.Remove(steering);
    }
}
