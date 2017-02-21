using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class Radar : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private float _updateFrequency;

    [SerializeField]
    private float _radius;

    [SerializeField]
    private float _fieldOfView = -0.5f;

    [SerializeField]
    private LayerMask _neighborLayerMask;
#pragma warning restore 0067, 0649

    private List<Agent> _neighbors = new List<Agent>();
    private Agent _self;

    public List<Agent> Neighbors
    {
        get { return _neighbors; }
    }

    void Awake()
    {
        _self = GetComponent<Agent>();
    }

    void OnEnable()
    {
        StartCoroutine(UpdateRadar());
    }

    IEnumerator UpdateRadar()
    {
        while (true)
        {
            _neighbors.Clear();
            Collider[] colliders = Physics.OverlapSphere(_self.transform.position, _radius, _neighborLayerMask.value);

            foreach (var collider in colliders)
            {
                Agent agent = collider.GetComponent<Agent>();
                if (agent)
                {
                    if (Vector3.Dot(_self.transform.forward, agent.transform.forward) > _fieldOfView)
                    {
                        _neighbors.Add(agent);
                    }
                }
            }
            yield return new WaitForSeconds(1.0f / _updateFrequency);
        }
    }
}

