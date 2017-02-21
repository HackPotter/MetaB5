using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Agent))]
[RequireComponent(typeof(AgentBehavior))]
public class TestBehaviorChooser : MonoBehaviour
{
    private Agent _agent;
    private AgentBehavior _agentBehavior;

    void Awake()
    {
        _agent = GetComponent<Agent>();
        _agentBehavior = GetComponent<AgentBehavior>();

        _agent.ActiveBehavior = _agentBehavior;
    }
}

