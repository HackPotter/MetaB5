using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;

// TODO we can't use System.Xml.Linq because Unity doesn't support it.

// TODO we need a well-defined interface between the server and client.

public class WebXmlFieldRetriever
{
    public WebXmlFieldRetriever()
    {
    }

    public Dictionary<string, object> GetFieldsFromXML(string xml)
    {
        Dictionary<string, object> fields = new Dictionary<string, object>();
        /**
        XDocument xmlDocument = XDocument.Load(new StringReader(xml));

        foreach (XElement element in xmlDocument.Nodes())
        {
            GetFieldsFromElement(element, fields, "");
        }
        */
        return fields;
    }
    /*
    private void GetFieldsFromElement(XElement element, Dictionary<string, object> dictionary, string currentLevel)
    {
        if (element.HasElements)
        {
            foreach (XElement child in element.Nodes())
            {
                GetFieldsFromElement(child, dictionary, currentLevel + element.Name + ".");
            }
            return;
        }

        string value = element.Value;
        string key = currentLevel + element.Name;

        if (IsNullValueElement(element))
        {
            dictionary.Add(key, null);
            return;
        }

        Type elementType = GetTypeFromTypeAttribute(element);

        if (elementType == typeof(int))
        {
            int elementValue;
            bool success = int.TryParse(element.Value, out elementValue);

            dictionary.Add(key, elementValue);
        }
        else if (elementType == typeof(DateTime))
        {
            dictionary.Add(key, DateTime.Parse(element.Value));
        }
        else if (elementType == typeof(string))
        {
            dictionary.Add(key, element.Value);
        }
    }

    private bool IsNullValueElement(XElement element)
    {
        XAttribute nilAttribute = element.Attribute("nil");
        if (nilAttribute != null)
        {
            bool nilResult;
            if (bool.TryParse(nilAttribute.Value, out nilResult))
            {
                return nilResult;
            }
        }
        return false;
    }

    private Type GetTypeFromTypeAttribute(XElement element)
    {
        XAttribute typeAttribute = element.Attribute("type");

        if (typeAttribute == null)
        {
            return typeof(String);
        }

        switch (typeAttribute.Value)
        {
            case "integer":
                return typeof(int);
            case "datetime":
                return typeof(DateTime);
            default:
                return typeof(string);
        }
    }
    */
}

