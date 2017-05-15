using UnityEngine;

public class HingeJointBuilder : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Vector3 _hingeChainSegmentOffset = Vector3.forward;
    [SerializeField]
    private int _numberOfSegments = 2;
    [SerializeField]
    private bool _removeFirstHinge = false;
#pragma warning restore 0067, 0649

    public GameObject Prefab
    {
        get { return _prefab; }
    }

    public Vector3 HingeChainSegmentOffset
    {
        get { return _hingeChainSegmentOffset; }
    }

    public int SegmentCount
    {
        get { return _numberOfSegments; }
    }


    public void BuildChain()
    {
        Vector3 position = Vector3.zero;
        Vector3 offsetStep = HingeChainSegmentOffset;

        // Create the chain root object. All elements will be under this game object.
        GameObject rootGO = new GameObject("JointChain");
        rootGO.transform.parent = transform;
        rootGO.transform.localPosition = Vector3.zero;
        rootGO.transform.localRotation = Quaternion.identity;

        // Create the first chain segment GameObject. Destroy the hinge joint (because it doesn't need it) and set the parent and position.
        GameObject chainSegmentGO = GameObject.Instantiate(Prefab) as GameObject;
        chainSegmentGO.name = Prefab.name + " 1";
        chainSegmentGO.transform.parent = rootGO.transform;
        chainSegmentGO.transform.localPosition = Vector3.zero;
        chainSegmentGO.transform.localRotation = Quaternion.identity;

        if (_removeFirstHinge)
        {
            GameObject.DestroyImmediate(chainSegmentGO.GetComponent<Joint>());
        }

        for (int i = 1; i < SegmentCount; i++)
        {
            // Increment the position
            position += offsetStep;

            // Create the next segment
            GameObject nextSegment = GameObject.Instantiate(Prefab) as GameObject;
            nextSegment.name = Prefab.name + (i + 1);

            // Parent the next segment with the root and set the position
            nextSegment.transform.parent = rootGO.transform;
            nextSegment.transform.localPosition = position;
            nextSegment.transform.localRotation = Quaternion.identity;

            // Setup the connected body to the previous chain
            nextSegment.GetComponent<Joint>().connectedBody = chainSegmentGO.GetComponent<Rigidbody>();

            chainSegmentGO = nextSegment;
        }
    }
}
