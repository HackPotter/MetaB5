// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
    Class: XMLNode
    Stores information for one XML node
*/
public class XMLNode
{
    public string tagName;
    public XMLNode parentNode;
    public List<XMLNode> children;
    public Hashtable attributes;
    public string value;

    /*
        Constructor: XMLNode()
        Initializes a blank XMLNode
    */
    public XMLNode()
    {
        tagName = "NONE";
        parentNode = null;
        children = new List<XMLNode>();
        attributes = new Hashtable();
        value = "";
    }

    /*
        Constructor: XMLNode(string, XMLNode)
        Initializes a named XMLNode and attaches it to a parent XMLNode
    */
    public XMLNode(string name, XMLNode parent)
    {
        tagName = name;
        parentNode = parent;
        parentNode.addNode(this);
        children = new List<XMLNode>();
        attributes = new Hashtable();
        value = "";
    }

    /*
        Function: addNode
	
        Adds a child node
	
        Parameters:
	
        newChild - The child XMLNode to attach	
    */
    void addNode(XMLNode newChild)
    {
        children.Add(newChild);
    }

    XMLNode XPath(string strPath)
    {
        return null;
    }
}
