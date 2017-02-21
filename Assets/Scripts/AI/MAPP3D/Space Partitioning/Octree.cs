// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//an enum to represent a node's octant value
public enum Octant { ROOT, OCT000, OCT010, OCT001, OCT011, OCT100, OCT110, OCT101, OCT111 }

/*
    Class: OTNode
    class to represent one octree node
*/
public class OTNode
{
    public Bounds otbounds;		//Unity Bounds object representing the geometry of this node
    public OTNode otparent;		//the parent of this node; NULL if this node is the root
    public List<OTNode> otchildren;			//array of all children of this node
    public List<OTNode> otneighbors;		//array of all neighboring nodes of this node
    public bool free;			//true if this node is traversable; false if not
    public Octant octant;			//if not the root, a value to represent position relative to other "siblings"
    public int depth;					//the depth at which is node is located

    /*
        Function: OTNode
        constructor
    */
    public OTNode(Bounds otb)
    {
        this.otbounds = otb;
        this.otchildren = new List<OTNode>();
        this.otneighbors = new List<OTNode>();
        this.free = false;
        this.depth = 0;
    }
}

/*
    Class: OTNode
    class to represent an octree
*/
public class Octree
{
    private OTNode rootNode;	//the root of this octree
    private Vector3 minVol;		//vector representing the minimum volume allowed for this node
    private List<OTNode> freeLeaves;
    private List<OTNode> fullLeaves;
    private int treeDepth;			//the depth of this octree
    private string layer;

    /*
        Function: Octree
        constructor
    */
    public Octree(Bounds rootBounds, Vector3 volMin)
    {
        rootNode = new OTNode(rootBounds);
        rootNode.octant = Octant.ROOT;

        minVol = volMin;
        freeLeaves = new List<OTNode>();
        fullLeaves = new List<OTNode>();
        treeDepth = 0;
    }

    /*
        Function: GetFreeLeaves
        returns leaves that are free
		
        Returns:
        free leaves of the octree
    */
    public List<OTNode> GetFreeLeaves()
    {
        return freeLeaves;
    }

    /*
        Function: GetFullLeaves
        returns leaves that are not free (full)
		
        Returns:
        full leaves of the octree
    */
    public List<OTNode> GetFullLeaves()
    {
        return fullLeaves;
    }

    /*
        Function: GetRoot
        returns the root of the octree
		
        Returns:
        rootNode
    */
    public OTNode GetRoot()
    {
        return rootNode;
    }

    /*
        Function:  GetTreeDepth
        gets the tree depth
		
        Returns:
            treeDepth
    */
    public int GetTreeDepth()
    {
        return treeDepth;
    }

    /*
        Function:  BuildOctree
        begins octree construction
    */
    public void BuildOctree()
    {
        Build(rootNode);
        BeginSearchForNeighbors(rootNode);
    }

    /*
        Function: SetLayer
		
        Parameters:
            layer - name of layer to cast against during construction
    */
    public void SetLayer(string layer)
    {
        this.layer = layer;
    }

    /*
        Function: VolumeAllowed
        returns true if the volume of node is allowed based on minVol
		
        Parameters:
            node - node to check against minimum volume
			
        Returns:
            true if volume allowed; false if volume is not allowed
    */
    private bool VolumeAllowed(Vector3 node)
    {
        if ((node.x < minVol.x) || (node.y < minVol.y) || (node.z < minVol.z))
        {
            return false;
        }

        return true;
    }

    /*
        Function: ContainsGeometry
        checks if the node contains any obstacles
		
        Parameters:
            node - node to check for geometry
			
        Returns:
            returns true if node contains any geometry; false if not
    */
    private bool ContainsGeometry(Bounds node)
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer(layer);//fix: must be dynamic

        //get longest side of bounds to use as the height of the capsule
        float h = Mathf.Max(node.extents.x, node.extents.y, node.extents.z);

        //get radius and start/end points of capsule
        float r;
        Vector3 start;
        Vector3 end;
        if (h == node.extents.x)
        {
            r = Mathf.Max(node.extents.y, node.extents.z);
            start = node.center;
            start.x -= (h - r);
            end = node.center;
            end.x += (h - r);
        }
        else if (h == node.extents.y)
        {
            r = Mathf.Max(node.extents.x, node.extents.z);
            start = node.center;
            start.y -= (h - r);
            end = node.center;
            end.y += (h - r);
        }
        else
        {
            r = Mathf.Max(node.extents.x, node.extents.y);
            start = node.center;
            start.z -= (h - r);
            end = node.center;
            end.z += (h - r);
        }

        //check capsule for collision
        return Physics.CheckCapsule(start, end, r, layerMask);

    }


    /*
        Function: Build
        1st part of the recursive algorithm:
        If node contains geometry then pass node to BuildNewNodes( ); otherwise, the node is marked as free
		
        Parameters:
            node - node to build from
    */
    private void Build(OTNode node)
    {
        if (ContainsGeometry(node.otbounds))
        {
            BuildNewNodes(node);
        }
        else
        {
            //this node is free
            node.free = true;
            freeLeaves.Add(node);
        }
    }

    /*
        Function: BuildNewNodes
        2nd part of the recursive algorithm:
        If the volume of node's children is allowed, then create node's children
        otherwise, node is a leaf containing geometry
		
        Parameters:
            node - node to build from
    */
    private void BuildNewNodes(OTNode node)
    {
        Vector3 newsize = new Vector3(node.otbounds.size.x / 2.0f, node.otbounds.size.y / 2.0f, node.otbounds.size.z / 2.0f);

        if (VolumeAllowed(newsize))
        {
            int curDepth = node.depth + 1;	//the current depth
            if (curDepth > treeDepth)
            {
                treeDepth = curDepth;
            }

            OTNode new000 = new OTNode(new Bounds(new Vector3((node.otbounds.center.x - node.otbounds.size.x / 4.0f), (node.otbounds.center.y - node.otbounds.size.y / 4.0f), (node.otbounds.center.z + node.otbounds.size.z / 4.0f)), newsize));
            new000.octant = Octant.OCT000;
            node.otchildren.Add(new000);
            new000.otparent = node;
            new000.depth = curDepth;
            Build(new000);

            OTNode new010 = new OTNode(new Bounds(new Vector3((node.otbounds.center.x + node.otbounds.size.x / 4.0f), (node.otbounds.center.y - node.otbounds.size.y / 4.0f), (node.otbounds.center.z + node.otbounds.size.z / 4.0f)), newsize));
            new010.octant = Octant.OCT010;
            node.otchildren.Add(new010);
            new010.otparent = node;
            new010.depth = curDepth;
            Build(new010);

            OTNode new001 = new OTNode(new Bounds(new Vector3((node.otbounds.center.x - node.otbounds.size.x / 4.0f), (node.otbounds.center.y + node.otbounds.size.y / 4.0f), (node.otbounds.center.z + node.otbounds.size.z / 4.0f)), newsize));
            new001.octant = Octant.OCT001;
            node.otchildren.Add(new001);
            new001.otparent = node;
            new001.depth = curDepth;
            Build(new001);

            OTNode new011 = new OTNode(new Bounds(new Vector3((node.otbounds.center.x + node.otbounds.size.x / 4.0f), (node.otbounds.center.y + node.otbounds.size.y / 4.0f), (node.otbounds.center.z + node.otbounds.size.z / 4.0f)), newsize));
            new011.octant = Octant.OCT011;
            node.otchildren.Add(new011);
            new011.otparent = node;
            new011.depth = curDepth;
            Build(new011);

            OTNode new100 = new OTNode(new Bounds(new Vector3((node.otbounds.center.x - node.otbounds.size.x / 4.0f), (node.otbounds.center.y - node.otbounds.size.y / 4.0f), (node.otbounds.center.z - node.otbounds.size.z / 4.0f)), newsize));
            new100.octant = Octant.OCT100;
            node.otchildren.Add(new100);
            new100.otparent = node;
            new100.depth = curDepth;
            Build(new100);

            OTNode new110 = new OTNode(new Bounds(new Vector3((node.otbounds.center.x + node.otbounds.size.x / 4.0f), (node.otbounds.center.y - node.otbounds.size.y / 4.0f), (node.otbounds.center.z - node.otbounds.size.z / 4.0f)), newsize));
            new110.octant = Octant.OCT110;
            node.otchildren.Add(new110);
            new110.otparent = node;
            new110.depth = curDepth;
            Build(new110);

            OTNode new101 = new OTNode(new Bounds(new Vector3((node.otbounds.center.x - node.otbounds.size.x / 4.0f), (node.otbounds.center.y + node.otbounds.size.y / 4.0f), (node.otbounds.center.z - node.otbounds.size.z / 4.0f)), newsize));
            new101.octant = Octant.OCT101;
            node.otchildren.Add(new101);
            new101.otparent = node;
            new101.depth = curDepth;
            Build(new101);

            OTNode new111 = new OTNode(new Bounds(new Vector3((node.otbounds.center.x + node.otbounds.size.x / 4.0f), (node.otbounds.center.y + node.otbounds.size.y / 4.0f), (node.otbounds.center.z - node.otbounds.size.z / 4.0f)), newsize));
            new111.octant = Octant.OCT111;
            node.otchildren.Add(new111);
            new111.otparent = node;
            new111.depth = curDepth;
            Build(new111);

        }
        else
        {
            //this node is full
            fullLeaves.Add(node);
        }

    }

    /*
        Function: BeginSearchForNeighbors
        1st part of neighbors recursive search algorithm:
		
        Begin search on an unsearched node	
		
        Parameters:
            node - node to search for neighbors
    */
    private void BeginSearchForNeighbors(OTNode node)
    {
        if (node.depth >= treeDepth)
        {
            return;
        }

        if (node.otchildren.Count != 0)
        {
            foreach (OTNode child in node.otchildren)
            {
                FindNeighbors(child, rootNode);
                BeginSearchForNeighbors(child);
            }
        }
    }

    /*
        Function: FindNeighbors
        2nd part of neighbors recursive search algorithm:
		
        recursively checks for neighbors
		
        Parameters:
            node - first node to compare against
            target - second node to compare the first node to
    */
    private void FindNeighbors(OTNode node, OTNode target)
    {
        if (target.otchildren.Count != 0)
        {
            foreach (OTNode targetChild in target.otchildren)
            {
                if (AreNeighbors(node.otbounds, targetChild.otbounds))
                {
                    node.otneighbors.Add(targetChild);
                }
                FindNeighbors(node, targetChild);
            }
        }
    }

    /*
        Function: AreNeighbors
        checks if target and node have intersecting bounds
		
        Parameters:
            node - first node to compare against
            target - second node to compare the first node to
			
        Returns:
            true if node and target intersect
    */
    private bool AreNeighbors(Bounds node, Bounds target)
    {
        if (node.Contains(target.center) || target.Contains(node.center))
        {
            return false;
        }

        return node.Intersects(target);
    }

    /*
        Function: TransformToNode
        gets the leaf node that contains the point
		
        Parameters:
            point - point to transform
		
        Returns:
            leaf node that contains the point
		
    */
    public OTNode TransformToNode(Vector3 point)
    {
        OTNode temp = rootNode;

        while (temp.otchildren.Count != 0)
        {
            foreach (OTNode child in temp.otchildren)
            {
                if (child.otbounds.Contains(point))
                {
                    temp = child;
                    break;
                }
            }
        }

        return temp;
    }
}
