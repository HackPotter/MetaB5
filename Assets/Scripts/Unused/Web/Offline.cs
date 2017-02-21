// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: Offline
	Holds methods and variables that are needed by the <Web> module when in Offline mode.
*/
class Offline
{
    // Constant: questionsPath
    // Path to the Questions.xml file
    private static string questionsPath = "file://" + Application.dataPath + "/Resources/Data/Questions.xml";

    private XMLNode rootQuestionsNode;
    private Action<Hashtable> loadQuestionsCallback;

    /*
            Constructor: Offline
            Initializes object and sets up Offline mode
    */
    public Offline()
    {
    }

    /*
        Function: GetQuestionsPath	
	
        Returns:
	
        string file path to Questions.xml file
    */
    public string getQuestionsPath() { return questionsPath; }

    public void loadOfflineQuestionsCallback(Hashtable returnData)
    {
        rootQuestionsNode = (XMLNode)returnData["data"];
        loadQuestionsCallback(returnData);
    }

    public void setLoadQuestionsCallback(Action<Hashtable> callback) { loadQuestionsCallback = callback; }
}
