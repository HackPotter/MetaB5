// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.IO;

/*
	Class: XMLWriter
	Writes an XML file from an <XMLNode>
*/
class XMLWriter
{
    /*
        Function: write
	
        Writes an <XMLNode> structure to a file
	
        Parameters:
	
        filePath - The target write path	
        rootNode - The root <XMLNode> of the XML tree being written
    */
    void write(string filePath, XMLNode rootNode)
    {
        //Put begin file write stuff here
        //Init the new file and write the root node to it
        var sw = new StreamWriter(filePath);
        traverseTree(rootNode, sw, 0);
        sw.Close();
    }

    /*
        Function: traverseTree
	
        Recursively traverses an <XMLNode> and writes the contents of the node to a StreamWriter
	
        Parameters:
	
        curNode - The current <XMLNode> being processed
        streamWriter - The StreamWriter that is writing the file
        tabNum - The number of tabs in the current line. Used to format the document
		
        See Also:
		
        - <traverseTree>
    */
    private void traverseTree(XMLNode curNode, StreamWriter streamWriter, int tabNum)
    {
        string output = "";
        string tabs = "";

        //Add tabs
        for (int i = 0; i < tabNum; i++)
        {
            tabs += "\t";
        }
        output += tabs + "<" + curNode.tagName;

        //add attributes
        if (curNode.attributes != null)
        {
            foreach (DictionaryEntry p in curNode.attributes)
            {
                output += " " + p.Key.ToString() + "=\"" + p.Value.ToString() + "\"";
            }
        }

        if ((curNode.children.Count == 0 || curNode.children == null) && (curNode.value == "" || curNode.value == null))
        {
            output += "/>";
            streamWriter.WriteLine(output);
        }
        else
        {
            //Debug.Log(curNode.tagName + "'" + curNode.value + "'" + curNode.children.length + "'");
            output += ">";
            if (curNode.value != "")
            {
                output += curNode.value;
                if (curNode.children == null || curNode.children.Count == 0)
                    output += "</" + curNode.tagName + ">";
            }
            streamWriter.WriteLine(output);
            if (curNode.children.Count != 0)
            {
                foreach (XMLNode child in curNode.children)
                {
                    traverseTree(child, streamWriter, tabNum + 1);
                }
                streamWriter.WriteLine(tabs + "</" + curNode.tagName + ">");
            }
        }
    }
}
