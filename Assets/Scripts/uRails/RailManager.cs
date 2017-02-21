using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class RailManager : MonoBehaviour
{
    public RailNode FirstNode;
    public float TargetSpeed;
    public float SpeedVariation;

    public float SegmentResolution = 0.05f;

    public float tau = 0.2f;

    void Start()
    {
        if (FirstNode == null)
        {
            Debug.Log("Rail null in RailManager!");
        }
    }



    // Editor stuff. TODO put in editor script.

    public bool GizmoEnabled = false;
    public bool ShowSegmentMarkers = false;
    public float SphereRadius = 5.0f;

    private Dictionary<RailNode, RailNode> visited = new Dictionary<RailNode, RailNode>();

    void OnDrawGizmos()
    {
        if (FirstNode == null || !GizmoEnabled)
        {
            return;
        }

        visited.Clear();
        DrawGizmoForAllNodes(FirstNode, visited);
    }

    void DrawGizmoForAllNodes(RailNode node, Dictionary<RailNode, RailNode> visited)
    {
        if (node == null || visited.ContainsKey(node))
            return;

        visited.Add(node, node);

        DrawGizmoLineForNode(node, visited);
        DrawGizmoForAllNodes(node.SuccessorNode1, visited);
        DrawGizmoForAllNodes(node.SuccessorNode2, visited);
    }

    void DrawGizmoLineForNode(RailNode node, Dictionary<RailNode, RailNode> visited)
    {
        // Draw the 8 possible iterations of this path given alt routes which are:

        // Main Main Main Main
        // Main Main Main Alt
        // Main Main Alt Main
        // Main Main Alt Alt
        // Main Alt Main Main
        // Main Alt Main Alt
        // Main Alt Alt Main
        // Main Alt Alt Alt
        if (node == null) return;

        RailNode node1, node2, node3;
        node1 = node.SuccessorNode1;
        if (node1 != null)
        {
            node2 = node1.SuccessorNode1;
            if (node2 != null)
            {
                node3 = node2.SuccessorNode1;
                if (node3 != null)
                {
                    DrawGizmoLineFromNodes(node, node1, node2, node3);
                }

                node3 = node2.SuccessorNode2;
                if (node3 != null)
                {
                    DrawGizmoLineFromNodes(node, node1, node2, node3);
                }
            }

            node2 = node1.SuccessorNode2;
            if (node2 != null)
            {
                node3 = node2.SuccessorNode1;
                if (node3 != null)
                {
                    DrawGizmoLineFromNodes(node, node1, node2, node3);
                }

                node3 = node2.SuccessorNode2;
                if (node3 != null)
                {
                    DrawGizmoLineFromNodes(node, node1, node2, node3);
                }
            }
        }

        node1 = node.SuccessorNode2;
        if (node1 != null)
        {
            node2 = node1.SuccessorNode1;
            if (node2 != null)
            {
                node3 = node2.SuccessorNode1;
                if (node3 != null)
                {
                    DrawGizmoLineFromNodes(node, node1, node2, node3);
                }

                node3 = node2.SuccessorNode2;
                if (node3 != null)
                {
                    DrawGizmoLineFromNodes(node, node1, node2, node3);
                }
            }

            node2 = node1.SuccessorNode2;
            if (node2 != null)
            {
                node3 = node2.SuccessorNode1;
                if (node3 != null)
                {
                    DrawGizmoLineFromNodes(node, node1, node2, node3);
                }

                node3 = node2.SuccessorNode2;
                if (node3 != null)
                {
                    DrawGizmoLineFromNodes(node, node1, node2, node3);
                }
            }
        }
    }

    void DrawGizmoLineFromNodes(RailNode node0, RailNode node1, RailNode node2, RailNode node3)
    {
        Vector3 p0 = node0.transform.position;
        Vector3 p1 = node1.transform.position;
        Vector3 p2 = node2.transform.position;
        Vector3 p3 = node3.transform.position;
        DrawGizmoLineFromPoints(p0, p1, p2, p3, SegmentResolution);
    }

    void DrawGizmoLineFromPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float delta)
    {
        if (delta == 0)
            return;

        for (float t = 0; t < 1; t += delta)
        {
            Vector3 start = MathExt.CatmullRom2(p0, p1, p2, p3, t, tau);
            Vector3 end = MathExt.CatmullRom2(p0, p1, p2, p3, t + delta, tau);

            float length = Vector3.Distance(start, end);

            float speed = length / delta;

            float deviation = (TargetSpeed - speed) / SpeedVariation;

            Color color;
            if (deviation > 0)
            {
                color = Color.Lerp(Color.green, Color.blue, deviation);
            }
            else if (deviation < 0)
            {
                color = Color.Lerp(Color.green, Color.red, -deviation);
            }
            else
            {
                color = Color.green;
            }

            Debug.DrawLine(start, end, color);
            if (ShowSegmentMarkers)
            {
                Gizmos.DrawSphere(start, SphereRadius);
            }
        }
    }
}

