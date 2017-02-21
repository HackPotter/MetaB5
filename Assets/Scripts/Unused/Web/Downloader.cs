// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

public class Downloader : MonoBehaviour
{
    /*
        Class: Downloader
        Responsible for downloading and returning data from the server. Instantiated by <Web>. Inherits from Unity <MonoBehaviour at http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour>
    */
    public Hashtable returnData;
    public WWW www;

    private const string serverErrorMsg = "We're sorry, but our servers are currently being updated. Please try again later or use the skip button to go to Offline Mode.";

    /*
        Function: loadURL(string, ReturnTypes, callback)

        Posts to a URL and initiates callback when done

        Parameters:

        url - URL string
        returnType - The type of data that needs to be returned
        callback - A function pointer to the method that will manage the returned data

        See Also:

        - <loadURL(string, WWWForm, ReturnTypes, callback)>
    */
    public void loadURL(string url, ReturnTypes returnType, Action<Hashtable> callback)
    {
        StartCoroutine(loadURL(url));
        while (!www.isDone) { }
        processReturnData(returnType);
        callback(returnData);
        Destroy(gameObject);
    }

    /*
        Function: loadURL(string, WWWForm, ReturnTypes, callback)

        Posts a form to a URL and initiates callback when done

        Parameters:

        url - URL string
        form - The WWWForm to submit
        returnType - The type of data that needs to be returned
        callback - A function pointer to the method that will manage the returned data

        See Also:

        - <loadURL(string, ReturnTypes, callback)>
    */
    public void loadURL(string url, WWWForm form, ReturnTypes returnType, Action<Hashtable> callback)
    {
        StartCoroutine(loadURL(url, form));
        while (!www.isDone) { }
        processReturnData(returnType);
        callback(returnData);
        Destroy(gameObject);
    }

    /*
        Function: processReturnData(ReturnTypes)

        Checks and formats the data that was returned from the server.

        Parameters:

        returnType - The type of data that needs to be returned
    */
    private void processReturnData(ReturnTypes returnType)
    {
        returnData = new Hashtable();

        if (www.error != null)
        {
            returnData["error"] = www.error;
        }
        else
        {
            switch (returnType)
            {
                case ReturnTypes.StringValue:
                    returnData["data"] = www.text;
                    break;
                case ReturnTypes.TextureValue:
                    returnData["data"] = www.texture;
                    break;
                case ReturnTypes.MovieValue:
                    returnData["data"] = www.movie;
                    break;
                case ReturnTypes.XMLValue:
                    XMLNode node;
                    XMLReader parser = new XMLReader();
                    node = parser.read(www.text);

                    if (node.tagName == "!DOCTYPE")
                    {
                        returnData["error"] = serverErrorMsg;
                    }
                    else if (node.tagName == "error")
                    {
                        foreach (XMLNode detail in node.children)
                        {
                            switch (detail.tagName)
                            {
                                case "description":
                                    returnData["error"] = detail.value;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        returnData["data"] = node;
                    }
                    break;
            }
        }
        //complete = true;
    }

    /*
        Function: loadURL 

        Submits the URL and waits until the server is finished

        Parameters:

        url - The target URL
	
        See Also:
	
        - <loadURL(string, ReturnTypes, callback)>
    */
    private IEnumerator loadURL(string url)
    {
        www = new WWW(url);

        yield return www;
    }

    /*
        Function: loadURL 

        Submits the WWWForm to a URL and waits until the server is finished

        Parameters:

        url - The target URL
        form - The WWWForm to submit to the URL
	
        See Also:
	
        - <loadURL(string, WWWForm, ReturnTypes, callback)>
    */
    private IEnumerator loadURL(string url, WWWForm form)
    {
        www = new WWW(url, form);

        yield return www;
    }
}