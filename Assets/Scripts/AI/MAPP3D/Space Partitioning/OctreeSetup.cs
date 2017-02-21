// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

/**
	This is used by the octree window for setting up the octree in the scene
*/

public class OctreeSetup : MonoBehaviour
{

    public Vector3 spaceCenter = new Vector3(0.0f, 0.0f, 0.0f);		//center of the octree
    public Vector3 spaceSize = new Vector3(1.0f, 1.0f, 1.0f);		//size of the bounds/root node of the octree
    public Vector3 minimumVolume = new Vector3(0.5f, 0.5f, 0.5f);	//the minimum volume allowed for the nodes of the octree
    public string layer = "";								//layer containing obstacles to check against
	
	
	[HideInInspector]
    public Octree octree;

    void Start()
    {

        /*Debug.Log("Octree \"" + gameObject.name + "\":" +
                    "\n\tcenter: " + spaceCenter.ToString() +
                    "\n\tsize: " + spaceSize.ToString() +
                    "\n\tmininim volume: " + minimumVolume.ToString());*/
		#if UNITY_EDITOR
        //initialize the octree		
        Bounds root = new Bounds(spaceCenter, spaceSize);
        octree = new Octree(root, minimumVolume);
        octree.SetLayer(layer);


        /*int startTime = Time.realtimeSinceStartup;
        octree.BuildOctree();
        int endTime = Time.realtimeSinceStartup;
        Debug.Log("Time to Build Tree: " + Mathf.Floor((endTime - startTime)/60) + " min(s) and " + (endTime - startTime)%60 + " sec(s).");
        Debug.Log("Number of free leaves: " + octree.GetFreeLeaves().length);
        Debug.Log("Number of full leaves: " + octree.GetFullLeaves().length);
        Debug.Log("Number of Leaves = " + (octree.GetFullLeaves().length + octree.GetFreeLeaves().length));
        Debug.Log("Depth = " + octree.GetTreeDepth());*/
		#endif
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(spaceCenter, spaceSize);

        Gizmos.color = Color.green;
        List<OTNode> fullLeaves = octree.GetFullLeaves();
        foreach (var node in fullLeaves)
        {
            Gizmos.DrawWireCube(node.otbounds.center, node.otbounds.size);
        }
        /*
        Gizmos.color = Color.gray;
        Array freeLeaves = octree.GetFreeLeaves();
        foreach(var node in freeLeaves){
            Gizmos.DrawWireCube(node.otbounds.center, node.otbounds.size);
        }*/
    }
}