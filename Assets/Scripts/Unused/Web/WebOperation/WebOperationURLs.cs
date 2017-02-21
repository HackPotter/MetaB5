using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// WebOperationURLs loads a text-based configuration file that maps web operation users to URLs.
/// </summary>
public sealed class WebOperationURLs
{
    // Relative path to packed resource file.
    private const string kURL_PATH = "Web/WebOperations";

    // Singleton instance.
    private static WebOperationURLs _webOperationURLs;

    /// <summary>
    /// Returns the string URL for a given type. 
    /// </summary>
    /// <param name="type">The type for which we are retrieving the operation URL.</param>
    /// <returns>A URL that can be used with WWW to perform an operation, or null if no such URL is found.</returns>
    public static string GetURL(Type type)
    {
        if (_webOperationURLs == null)
        {
            _webOperationURLs = new WebOperationURLs();
        }
        return _webOperationURLs.GetURLFromType(type);
    }

    // Dictionary containing URLs keyed by type string.
    private Dictionary<string, string> _urlsByType = new Dictionary<string, string>();

    /// <summary>
    /// Constructs a WebOperationURLs object.
    /// </summary>
    private WebOperationURLs()
    {
        TextAsset text = (TextAsset)Resources.Load(kURL_PATH, typeof(TextAsset));
        Asserter.NotNull(text, "WebOperationURLs.ctor:failed to load WebOperations.txt");

        StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(text.text)));
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] splitLine = line.Split(' ');

            Asserter.IsTrue(splitLine.Length == 2, "WebOperationURLs.ctor:unexpected input from WebOperations.txt");
            _urlsByType.Add(splitLine[0], splitLine[1]);
        }
    }

    /// <summary>
    /// Returns the WebOperation URL associated with the given type, or null if no URL is found.
    /// </summary>
    /// <param name="type">The type for which to return a WebOperation URL.</param>
    /// <returns>The URL of the operation.</returns>
    private string GetURLFromType(Type type)
    {
        if (!_urlsByType.ContainsKey(type.FullName))
        {
            Asserter.IsTrue(false, "WebOperationURLs.GetURLFromType:WebOperations.txt contains no URL mapping for given Type (" + type.Name + ")");
        }
        return _urlsByType[type.FullName];
    }
}

