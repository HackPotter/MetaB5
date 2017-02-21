// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

/*
	Class: XMLReader
	Reads an XML file
*/
public class XMLReader
{
	private const string XML_START = "<?xml";
	private const char TAG_START = '<';
	private const char TAG_END = '>';
	private const char SPACE = ' ';
	private const char QUOTE = '"';
	private const char SLASH = '/';
	private const char EQUALS = '=';
	private const string BEGIN_QUOTE = "=\"";
		
	/*
		Function: read
	
		Reads an XML file into an <XMLNode>
	
		Parameters:
	
		xml - The xml string 
		
		Returns:
		
		An <XMLNode> representing the root node of the XML string
	*/ 
	public XMLNode read ( string xml  ){
		int index = 0;
		int lastIndex = 0;
		XMLNode rootNode = new XMLNode();
		XMLNode currentNode = rootNode;
		int startValIndex = 0;
		int endValIndex = 0;
		
		index = xml.IndexOf(XML_START, lastIndex);
		if (index >= 0) {
			index = xml.IndexOf(TAG_END,lastIndex);
		}
		index++;
		lastIndex = index;  //This should skip the XML declaration
		
		while (true) {			
			index = xml.IndexOf(TAG_START, lastIndex);
			if (index < 0 || index >= xml.Length) break;
			index++; // skip the tag-char
				
			lastIndex = xml.IndexOf(TAG_END, index);
			if (lastIndex < 0 || lastIndex >= xml.Length) break;
			
			int tagLength= lastIndex - index;
			string xmlTag = xml.Substring(index, tagLength);
			
			// The tag starts with "</", it is thus an end tag
			if (xmlTag[0] == SLASH) {
				if(startValIndex > 0){
					endValIndex = index - 1;
					if(currentNode.children.Count < 1)
						currentNode.value = ReplaceXMLEntities(xml.Substring(startValIndex, endValIndex - startValIndex));
				}
 				currentNode = currentNode.parentNode;  // go up to parent node.  we are done here
				continue;
			}
			
			bool openTag= true;
			// The tag ends in "/>", it is thus a closed tag
			if (xmlTag[tagLength - 1] == SLASH) {
				currentNode.value = "";
				xmlTag = xmlTag.Substring(0, tagLength - 1); // cut away the slash
				openTag = false;
			}
			else
			{
				startValIndex = lastIndex + 1;
			}
			
			//s += readTag(xmlTag, indent);
			XMLNode node = parseTag(xmlTag);
			
			if(currentNode.tagName == "NONE")
			{
				currentNode = node;
				rootNode = currentNode;
			}
			else
			{
				node.parentNode = currentNode;
				currentNode.children.Add(node);
			}
			
			if (openTag) {
				currentNode = node;
			}
		};
		
		return rootNode;
	}
	
	/*
		Function: parseTag
	
		Creates an XML node from a chunk of an XML string
	
		Parameters:
	
		xmlTag - The xml string 
		
		Returns:
		
		<XMLNode>
	*/ 
	XMLNode parseTag ( string xmlTag  ){
		XMLNode node = new XMLNode();
	
		int nameEnd = xmlTag.IndexOf(SPACE, 0);
		if (nameEnd < 0) {
			node.tagName = xmlTag;
			return node;
		}
		
		string tagName = xmlTag.Substring(0, nameEnd);
		node.tagName = tagName;
		
		string attrString = xmlTag.Substring(nameEnd, xmlTag.Length - nameEnd);
		return parseAttributes(attrString, node);
	}

	void  removeSpaces (){
	
	}
	
	/*
		Function: parseAttributes
	
		Parses the XML attributes from an XML string
	
		Parameters:
	
		xmlTag - The xml string 
		node - The <XMLNode> to which the attributes belong
		
		Returns:
		
		The <XMLNode> parameter with all attributes attached
	*/ 
	XMLNode parseAttributes ( string xmlTag ,   XMLNode node  ){
		int index = 0;
		
		xmlTag = xmlTag.Trim();
		
		while (true) 
		{
			index = xmlTag.IndexOf(QUOTE);
			if (index < 0 || index > xmlTag.Length)	break;
			
			int equalIndex = xmlTag.IndexOf(EQUALS);
			int endQuote = xmlTag.IndexOf(QUOTE, index + 1);
			string attrValue= xmlTag.Substring(index + 1, endQuote - index - 1);
			
			if(equalIndex > 0)
			{
				string attrName = xmlTag.Substring(0, equalIndex).Trim();
				node.attributes[attrName] = attrValue;
			}
			xmlTag = xmlTag.Substring(endQuote + 1);
		}
		
		return node;
	}

	/**
		Replaces all XML entites (& > < " ') in a given 
		string with their corresponding characters.

		Params:
			content: the string to process
		Returns:
			The given string with any XML entities replaced
	*/
	string ReplaceXMLEntities ( string content  ){
		string result = content.Replace(">",">");
		result = result.Replace("<","<");
		result = result.Replace("\"","\"");
		result = result.Replace("'","'");
		result = result.Replace("&","&");
		return result;
	}
}
