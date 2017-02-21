// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
	Class: FollowPath
	Behavior that allows the agent to follow a path.

	See Also:
		SeekPoint
*/
[Serializable]
public class FollowPath : SeekPoint
{
    [SerializeField]
    public string octreeName;

    [SerializeField]
    public Transform target;

    protected Octree octree;
    protected AStar astar;
    protected List<Bounds> path;
    //protected int locOnPath;

    /*
        Function: FollowPath
        Constructor
		
    */
    public FollowPath()
    {
        astar = new AStar();
        path = new List<Bounds>();
        //locOnPath = -1;
    }

    /*
        Function: GetOctreeName
        Gets the name of the octree used for path generation
		
        Returns:
            a string representing the name of the octree
    */
    string GetOctreeName()
    {
        return octreeName;
    }

    /*
        Function: GetPath
        Gets the path currently being used
		
        Returns:
            an array of Bounds object representing the path to follow
    */
    List<Bounds> GetPath()
    {
        return path;
    }

    /*
        Function: SetOctreeName
        Sets the name of the octree to be used for path generation
		
        Parameters:
            octreeName - the name of the octree to be used
    */
    public void SetOctreeName(string octreeName)
    {
        this.octreeName = octreeName;
    }

    /*
        Function: HasPath
        Generates a path from the agent to the target via an A* search on the target
		
        Returns:
            true if there is a path; false if there is no path
    */
    private bool HasPath()
    {
        if (path.Count > 0 && path[path.Count - 1].Contains(target.position))
        {
            return true;
        }

        //generate new path to target
        //get the desired octree in scene
        GameObject[] octreeGOs = GameObject.FindGameObjectsWithTag("Octree");
        foreach (GameObject tree in octreeGOs)
        {
            if (tree.name.Equals(octreeName))
            {
                octree = tree.GetComponent<OctreeSetup>().octree;
                break;
            }
        }

        if (octree == null)
        {
            Debug.LogError("Octree does not exist");
            return false;
        }

        //get start and goal nodes
        OTNode goalNode = octree.TransformToNode(target.position);
        OTNode startNode = octree.TransformToNode(Agent.transform.position);
        astar.SetStart(startNode);
        astar.SetGoal(goalNode);

        //see if there is a path between start and goal
        if (astar.AStarSearch())
        {
            path = astar.GetPath();
            Debug.Log("Path has been generated!");
            Debug.Log("Path length: " + path.Count);
            return true;
        }

        Debug.Log("NO PATH GENERATED!!!");
        path.Clear();
        return false;
    }

    /*
        Function: GetAcceleration
        Computes the acceleration need for the agent to avoid a collision with other agents
		
        Returns:
            a 3D vector representing the collision needed.
    */
    public override Vector3 GetAcceleration()
    {
        if (!HasPath())
        {
            return Vector3.zero;
        }

        if (path.Count == 1)
        {
            base.SetPoint(target.position);
            return base.GetAcceleration();
        }

        //get the current location on the path
        Bounds currentLoc = path[0];

        if (currentLoc.Contains(Agent.transform.position))
        {
            path.RemoveAt(0);
            currentLoc = path[0];
        }

        base.SetPoint(currentLoc.center);
        return base.GetAcceleration();
    }
}
