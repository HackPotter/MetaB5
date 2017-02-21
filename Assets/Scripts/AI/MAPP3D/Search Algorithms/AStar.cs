// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
    Class: NodeRecord
    A* search crumbs to keep track of visited nodes
*/
public class NodeRecord
{
    public OTNode node;
    public NodeRecord connection;
    public float costFromStart;
    public float costToGoal;
    public float totalCost;
}

/*
    Class: AStar
    A* search algorithm to work with the octree data structure
*/
public class AStar
{
    List<NodeRecord> open;
    List<NodeRecord> closed;
    List<Bounds> path;
    OTNode start;
    OTNode goal;

    /*
        Function: AStar
        Constructor
    */
    public AStar()
    {
        open = new List<NodeRecord>();
        closed = new List<NodeRecord>();
        path = new List<Bounds>();
    }

    /*
        Function: SetStart
        Sets the start node
		
        Parameters:
            start - the start node
    */
    public void SetStart(OTNode start)
    {
        this.start = start;
    }

    /*
        Function: SetGoal
        Sets the goal node
		
        Parameters:
            goal - the goal node
    */
    public void SetGoal(OTNode goal)
    {
        this.goal = goal;
    }

    /*
        Function: GetPath
        Returns path between start and goal; if there is no path an empty array is returned
		
        Returns:
            path
    */
    public List<Bounds> GetPath()
    {
        return path;
    }

    /*
        Function: AStarSearch
        Perform a search for a path between the start and goal
		
        Returns:
            true if a path exists; false if a path does not exist
    */
    public bool AStarSearch()
    {
        //clear lists

        //Initialize the start node record
        NodeRecord startRecord = new NodeRecord();
        startRecord.node = start;
        startRecord.costFromStart = 0;
        startRecord.costToGoal = ComputeCostToGoal(start.otbounds.center);
        startRecord.totalCost = startRecord.costToGoal; //since cost from start is 0
        open.Add(startRecord);

        while (open.Count != 0)
        {
            NodeRecord currentRecord = open[0];//removes and returns first element of array
            open.RemoveAt(0);
            //if currentRecord holds goal then return success
            if (currentRecord.node == goal)
            {
                ConstructPath(currentRecord);
                return true;
            }
            else
            {
                foreach (OTNode neighbor in currentRecord.node.otneighbors)
                {
                    //if neighbor is not traversable, continue to the next neighbor
                    if (!neighbor.free)
                    {
                        continue;
                    }

                    float costFromStart = currentRecord.costFromStart + ComputeCostFromStart(neighbor.otbounds.center);
                    int locInOpen = (int)GetLocationInOpenList(neighbor);
                    int locInClosed = (int)GetLocationInClosedList(neighbor);

                    if (locInClosed >= 0)
                    {
                        if (closed[locInClosed].costFromStart <= costFromStart)
                        {
                            continue;
                        }
                        else
                        {
                            closed.RemoveAt(locInClosed); //remove node from closed
                        }
                    }
                    else if (locInOpen >= 0)
                    {
                        if (open[locInOpen].costFromStart <= costFromStart)
                        {
                            continue;
                        }
                        else
                        {
                            open.RemoveAt(locInOpen); //remove node from open
                        }
                    }

                    NodeRecord nodeRecord = new NodeRecord();
                    nodeRecord.node = neighbor;
                    nodeRecord.costFromStart = costFromStart;
                    nodeRecord.costToGoal = ComputeCostToGoal(neighbor.otbounds.center);
                    nodeRecord.totalCost = costFromStart + nodeRecord.costToGoal;
                    nodeRecord.connection = currentRecord;
                    InsertInOpen(nodeRecord);

                }//end for loop
            }

            //done inspecting potential connections of current node
            closed.Add(currentRecord);
        }//end while loop

        //if there are no more nodes on open, then return failure
        return false;
    }

    /*
        Function: ComputeCostToGoal
        computes the distance from pos to goal
		
        Parameters:
            pos - current position being inspected
			
        Returns:
            cost to goal
    */
    float ComputeCostToGoal(Vector3 pos)
    {
        //using euclidean distance
        Vector3 goalPos = goal.otbounds.center;

        return (pos - goalPos).magnitude;
    }

    /*
        Function: ComputeCostToStart
        computes the distance from pos to start
		
        Parameters:
            pos - current position being inspected
			
        Returns:
            cost to goal
    */
    float ComputeCostFromStart(Vector3 pos)
    {
        //using euclidean distance
        Vector3 startPos = start.otbounds.center;

        return (pos - startPos).magnitude;
    }

    /*
        Function: ConstructPath
        forms the path generated from A*
		
        Parameters:
            nodeRecord - a node record; if there is a path this will be the goal node
    */
    void ConstructPath(NodeRecord nodeRecord)
    {
        NodeRecord temp = nodeRecord;

        while (temp != null)
        {
            //path.push(temp.node.otbounds);
            path.Insert(0,temp.node.otbounds);
            temp = temp.connection;
        }
    }
    /*
        Function: GetLocationInOpenList
        gets the location of node on the open list
		
        Parameters:
            node - current node being inspected by A*
			
        Returns:
            the index of the location of node on the open list
    */
    float GetLocationInOpenList(OTNode node)
    {
        for (int i = 0; i < open.Count; i++)
        {
            if (open[i].node.Equals(node))
            {
                return i;
            }
        }

        return -1;
    }

    /*
        Function: GetLocationInClosedList
        gets the location of node on the closed list
		
        Parameters:
            node - current node being inspected by A*
			
        Returns:
            the index of the location of node on the closed list
    */
    float GetLocationInClosedList(OTNode node)
    {
        for (int i = 0; i < closed.Count; i++)
        {
            if (closed[i].node.Equals(node))
            {
                return i;
            }
        }
        return -1;
    }

    /*
        Function: InsertInOpen
        puts a node in the open list according to the total cost; open must remain sorted by total cost
		
        Parameters:
            node - current node being inspected by A*
    */
    void InsertInOpen(NodeRecord nodeRecord)
    {
        for (int i = 0; i < open.Count; i++)
        {
            if (nodeRecord.totalCost <= open[i].totalCost)
            {
                open.Insert(i, nodeRecord); //adds to open at i
                return;
            }
        }

        open.Add(nodeRecord);
    }
}
