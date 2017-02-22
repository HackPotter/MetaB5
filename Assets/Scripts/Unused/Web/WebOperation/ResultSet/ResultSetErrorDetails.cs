#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using System;
using System.Xml;
using System.IO;

public class ResultSetErrorDetails
{
    private int _errorId;
    private string _errorName;
    private string _errorDescription;
    private DateTime _createdAt;

    public static ResultSetErrorDetails CreateFromXML(string xml)
    {
        ResultSetErrorDetails details = new ResultSetErrorDetails();

        XmlReader reader = XmlReader.Create(new StringReader(xml));

        // XmlDeclaration
        reader.Read();
        // Whitespace
        reader.Read();

        //Table-name
        reader.Read();

        reader.ReadToFollowing("error-type-id");
        details._errorId = reader.ReadElementContentAsInt();

        reader.ReadToFollowing("name");
        details._errorName = reader.ReadElementContentAsString();

        reader.ReadToFollowing("description");
        details._errorDescription = reader.ReadElementContentAsString();

        reader.ReadToFollowing("created-at");
        details._createdAt = reader.ReadElementContentAsDateTime();

        reader.Close();

        return details;
    }
}
