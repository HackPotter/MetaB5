#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

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
