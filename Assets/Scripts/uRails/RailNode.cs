using UnityEngine;
using System.Collections;

public class RailNode : MonoBehaviour
{
    private int _nextNode = 0;

    public RailNode SuccessorNode1;
    public RailNode SuccessorNode2;

    public Vector2? Bounds;

    public RailNode NextNode
    {
        get { return _nextNode == 0 ? SuccessorNode1 : SuccessorNode2; }
    }

    public void SwitchPath()
    {
        _nextNode = 1 - _nextNode;
    }

    public void SwitchToMainPath()
    {
        _nextNode = 0;
    }

    public void SwitchToAltPath()
    {
        _nextNode = 1;
    }

    void OnDrawGizmos()
    {
        float size = (Camera.current.transform.position - transform.position).magnitude;

        if (SuccessorNode1 != null && SuccessorNode2 != null)
        {
            Gizmos.DrawSphere(transform.position, 0.025f * size);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, 0.025f * size * Vector3.one);
        }
        else if (SuccessorNode1 != null)
        {
            Gizmos.DrawSphere(transform.position, 0.02f * size);
        }
        else
        {
            Gizmos.DrawCube(transform.position, 0.02f * size * Vector3.one);
        }
    }

    void OnDrawGizmosSelected()
    {
        float size = (Camera.current.transform.position - transform.position).magnitude;

        if (SuccessorNode1 != null && SuccessorNode2 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.025f * size);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, 0.025f * size * Vector3.one);
        }
        else if (SuccessorNode1 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.02f * size);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, 0.02f * size * Vector3.one);
        }
    }
}
