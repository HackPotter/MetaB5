using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ForceFieldProximityUpdater : MonoBehaviour
{
    private Material _material;

#pragma warning disable 0067, 0649
    [SerializeField]
    private Transform _target;
#pragma warning restore 0067, 0649

    void Start ()
    {
        _material = GetComponent<Renderer>().material;
	}
	
	void Update ()
    {
        _material.SetVector("_ProximityLocation", _target.transform.position);
	}
}
