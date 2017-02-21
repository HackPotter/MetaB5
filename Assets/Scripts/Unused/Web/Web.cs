// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

public enum TimeInMenu { BioLog, Inventory, Objectives, MessageCenter, ClassManagement, QuestionManagement, MyClasses, MyQuestions, Main, Options }

public enum WebActions
{
    GetMemberships,
    RejectInvite,
    AcceptInvite,
    LeaveClass,
    FindClasses,
    RemoveClass,
    JoinClass,
    CreateClass,
    AcceptPendingMember,
    RejectPendingMember,
    InviteMember,
    RemoveMember,
    GetGroupMembers,
    GetPendingGroupMembers,
    GetQuestions,
    EditModule,
    CreateModule,
    RemoveModule,
    RemoveQuestion,
    AnswerQuestion,
    SaveQuestion,
    PostStats,
    PatchNotes
}

public enum ReturnTypes
{
    StringValue,
    TextureValue,
    MovieValue,
    XMLValue
}
/*
    Class: Web
    Module that is responsible for all communications with the web as well as redirection if in Offline mode. Inherits from Unity <MonoBehaviour at http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour>
*/
public class Web : MonoBehaviour
{

    private const string baseURL = "http://metablastapi.vrac.iastate.edu/api/";
    private const string devURL = "http://www.metablast.org:3300/api/";

    private const string serverErrorMsg = "We're sorry, but our servers are currently being updated. Please try again later or use the skip button to go to Offline Mode.";

    private int _userID;
    private string _authToken;
    private bool _online;
    private string _userName;

    public bool useDev;

    private WWW www;
    private Offline offline;

    private string[] Actions;

    void Start()
    {
        _userID = -1;
        _authToken = "";
        _online = false;
        _userName = "";

        //Init Actions
        Actions = new string[System.Enum.GetValues(typeof(WebActions)).Length];
        Actions[(int)WebActions.GetMemberships] = "game_users/get_groups";
        Actions[(int)WebActions.RejectInvite] = "groups/reject_invite";
        Actions[(int)WebActions.AcceptInvite] = "groups/accept_invite";
        Actions[(int)WebActions.LeaveClass] = "groups/remove_member";
        Actions[(int)WebActions.FindClasses] = "groups/get_by_user";
        Actions[(int)WebActions.RemoveClass] = "groups/remove";
        Actions[(int)WebActions.JoinClass] = "groups/join";
        Actions[(int)WebActions.CreateClass] = "groups/create";
        Actions[(int)WebActions.AcceptPendingMember] = "groups/accept_member";
        Actions[(int)WebActions.RejectPendingMember] = "groups/reject_member";
        Actions[(int)WebActions.InviteMember] = "groups/invite";
        Actions[(int)WebActions.RemoveMember] = "groups/remove_member";
        Actions[(int)WebActions.GetGroupMembers] = "groups/get_active_users";
        Actions[(int)WebActions.GetPendingGroupMembers] = "groups/get_pending_users";
        Actions[(int)WebActions.GetQuestions] = "questions/get_data";
        Actions[(int)WebActions.EditModule] = "collections/edit";
        Actions[(int)WebActions.CreateModule] = "collections/create";
        Actions[(int)WebActions.RemoveModule] = "collections/remove";
        Actions[(int)WebActions.RemoveQuestion] = "questions/remove";
        Actions[(int)WebActions.AnswerQuestion] = "questions/submit_answer";
        Actions[(int)WebActions.SaveQuestion] = "questions/submit";
        Actions[(int)WebActions.PostStats] = "statistics/add";
        Actions[(int)WebActions.PatchNotes] = "patch_notes";

        initStatTracking();
    }

    //In case anyone doesn't need a callback
    void DoNothing(Hashtable returnData) { }

    /*
        Function: getUsername

        Returns the current username
    */
    public string getUsername() { return _userName; }

    /*
        Function: getUserID

        Returns the current user identification number
    */
    public int getUserID() { return _userID; }

    /*
        Function: isOnline

        Returns true if the module is online
    */
    public bool isOnline() { return _online; }

    /*
        Function: setOnline

        Sets the online state of the module

        Parameters:

        online - The bool  indicating whether or not the module is online
    */
    public void setOnline(bool online) { _online = online; }

    /*
        Function: Login

        Logs the user in and saves authToken and userID if successful

        Parameters:

        username - The username used to log in
        password - The password used to log in

        Returns:

        A hashtable with "data" key set to true if successful and "error" key containing the error description if unsuccessful.

        See Also:

        - <Register>
    */
    public Hashtable Login(string username, string password)
    {
        string url;

        if (useDev)
            url = devURL + "game_users/login";
        else
            url = baseURL + "game_users/login";

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        StartCoroutine(loadSimpleForm(url, form));
        while (!www.isDone) { }

        Hashtable returnData = new Hashtable();

        if (www.error != null)
        {
            returnData["error"] = www.error;
        }
        else
        {
            XMLNode node;
            XMLReader parser = new XMLReader();

            Debug.Log(www.text);

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
                foreach (XMLNode detail in node.children)
                {
                    switch (detail.tagName)
                    {
                        case "token":
                            _authToken = detail.value;
                            break;
                        case "game-user-id":
                            _userID = int.Parse(detail.value);
                            break;
                    }
                }
                _online = true;
                _userName = username + "'s";
                returnData["data"] = true;
            }
        }

        return returnData;
    }
    /*
        Function: Register

        Registers a new user with the server

        Parameters:

        form - A form containing all of the necessary information for registering a new user (username, password, email, etc)

        Returns:

        A hashtable with "data" key set to true if successful and "error" key containing the error description if unsuccessful.
	
        See Also:

        - <Login>
    */
    public Hashtable Register(WWWForm form)
    {
        string url;

        if (useDev)
            url = devURL + "game_users/sign_up";
        else
            url = baseURL + "game_users/sign_up";

        Debug.Log(url);
        StartCoroutine(loadSimpleForm(url, form));
        while (!www.isDone) { }

        Debug.Log(www.text);

        Hashtable returnData = new Hashtable();

        if (www.error != null)
        {
            returnData["error"] = www.error;
        }
        else
        {
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
                            foreach (XMLNode child in detail.children)
                            {
                                switch (child.tagName)
                                {
                                    case "description":
                                        returnData["error"] = child.value;
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                returnData["data"] = true;
            }
        }

        return returnData;
    }

    /*
        Function: loadSimpleForm 

        Submits a WWWForm to a URL

        Parameters:

        url - The target URL
        form - The WWWForm to submit to the URL
    */
    private IEnumerator loadSimpleForm(string url, WWWForm form)
    {
        www = new WWW(url, form);

        yield return www;
    }

    /*
        Function: PostAction(WebActions, function) 

        Submits an action to the server

        Parameters:

        action - The desired action
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <PostAction(WebActions, ReturnTypes, function)>
        - <PostAction(WebActions, WWWForm)>
        - <PostAction(WebActions, WWWForm, function)>
        - <PostAction(WebActions, WWWForm, ReturnTypes, function)>
    */
    private void PostAction(WebActions action, Action<Hashtable> callback)
    {
        PostAction(action, new WWWForm(), ReturnTypes.StringValue, callback);
    }

    /*
        Function: PostAction(WebActions, ReturnTypes, function) 

        Submits an action to the server

        Parameters:

        action - The desired action
        returnType - The type of data that needs to be returned
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <PostAction(WebActions, function)>
        - <PostAction(WebActions, WWWForm)>
        - <PostAction(WebActions, WWWForm, function)>
        - <PostAction(WebActions, WWWForm, ReturnTypes, function)>
    */
    public void PostAction(WebActions action, ReturnTypes returnType, Action<Hashtable> callback)
    {
        PostAction(action, new WWWForm(), returnType, callback);
    }

    /*
        Function: PostAction(WebActions, WWWForm) 

        Submits an action to the server

        Parameters:

        action - The desired action
        form - The WWWForm to submit to the server
	
        See Also:
	
        - <PostAction(WebActions, function)>
        - <PostAction(WebActions, ReturnTypes, function)>
        - <PostAction(WebActions, WWWForm, function)>
        - <PostAction(WebActions, WWWForm, ReturnTypes, function)>
    */
    void PostAction(WebActions action, WWWForm form)
    {
        PostAction(action, form, ReturnTypes.StringValue, DoNothing);
    }

    /*
        Function: PostAction(WebActions, WWWForm, function) 

        Submits an action to the server

        Parameters:

        action - The desired action
        form - The WWWForm to submit to the server
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <PostAction(WebActions, function)>
        - <PostAction(WebActions, ReturnTypes, function)>
        - <PostAction(WebActions, WWWForm)>
        - <PostAction(WebActions, WWWForm, ReturnTypes, function)>
    */
    public void PostAction(WebActions action, WWWForm form, Action<Hashtable> callback)
    {
        PostAction(action, form, ReturnTypes.StringValue, callback);
    }

    /*
        Function: PostAction(WebActions, WWWForm, ReturnTypes, function) 

        Submits an action to the server

        Parameters:

        action - The desired action
        form - The WWWForm to submit to the server
        returnType - The type of data that needs to be returned
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <PostAction(WebActions, function)>
        - <PostAction(WebActions, ReturnTypes, function)>
        - <PostAction(WebActions, WWWForm)>
        - <PostAction(WebActions, WWWForm, function)>
    */
    public void PostAction(WebActions action, WWWForm form, ReturnTypes returnType, Action<Hashtable> callback)
    {
        if ((int)action < Actions.Length && action >= 0)
        {
            if (_online)
            {
                if (useDev)
                    loadURL(devURL + Actions[(int)action], form, returnType, callback);
                else
                    loadURL(baseURL + Actions[(int)action], form, returnType, callback);
            }
            else
            {
                postOfflineAction(action, form, returnType, callback);
            }
        }
        else
        {
            Hashtable returnData = new Hashtable();
            returnData["error"] = "Action " + action + " doesn't exist.";
            callback(returnData);
        }
    }

    /*
        Function: loadURL(string, function) 

        Posts to a url. This method can be used if you want to post to a URL that isn't a part of the Meta!Blast API. Any API calls should use PostAction.

        Parameters:

        url - The URL string to post to
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <PostAction(WebActions, function)>
    */
    void loadURL(string url, Action<Hashtable> callback)
    {
        loadURL(url, ReturnTypes.StringValue, callback);
    }

    /*
        Function: loadURL(string, WWWForm) 

        Posts to a url. This method can be used if you want to post to a URL that isn't a part of the Meta!Blast API. Any API calls should use PostAction.

        Parameters:

        url - The URL string to post to
        form - The WWWForm to submit to the server
	
        See Also:
	
        - <PostAction(WebActions, WWWForm)>
    */
    void loadURL(string url, WWWForm form)
    {
        loadURL(url, form, ReturnTypes.StringValue, DoNothing);
    }

    /*
        Function: loadURL(string, WWWForm, function) 

        Posts to a url. This method can be used if you want to post to a URL that isn't a part of the Meta!Blast API. Any API calls should use PostAction.

        Parameters:

        url - The URL string to post to
        form - The WWWForm to submit to the server
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <PostAction(WebActions, WWWForm, function)>
    */
    void loadURL(string url, WWWForm form, Action<Hashtable> callback)
    {
        loadURL(url, form, ReturnTypes.StringValue, callback);
    }

    /*
        Function: loadURL(string, ReturnTypes, function) 

        Posts to a url. This method can be used if you want to post to a URL that isn't a part of the Meta!Blast API. Any API calls should use PostAction.

        Parameters:

        url - The URL string to post to
        returnType - The type of data that needs to be returned
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <PostAction(WebActions, ReturnTypes, function)>
    */
    void loadURL(string url, ReturnTypes returnType, Action<Hashtable> callback)
    {
        if ((int)returnType < System.Enum.GetValues(typeof(ReturnTypes)).Length && returnType >= 0)
        {
            GameObject newStream = new GameObject("Stream");
            Downloader newDownloader = newStream.AddComponent<Downloader>();
            newStream.transform.parent = transform;
            newDownloader.loadURL(url, returnType, callback);
        }
        else
        {
            Hashtable returnData = new Hashtable();
            returnData["error"] = "Return type " + returnType + " doesn't exist.";
            callback(returnData);
        }
    }

    /*
        Function: loadURL(string, WWWForm, ReturnTypes, function) 

        Posts to a url. This method can be used if you want to post to a URL that isn't a part of the Meta!Blast API. Any API calls should use PostAction.

        Parameters:

        url - The URL string to post to
        form - The WWWForm to submit to the server
        returnType - The type of data that needs to be returned
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <PostAction(WebActions, WWWForm, ReturnTypes, function)>
    */
    void loadURL(string url, WWWForm form, ReturnTypes returnType, Action<Hashtable> callback)
    {
        if ((int)returnType < System.Enum.GetValues(typeof(ReturnTypes)).Length && returnType >= 0)
        {
            form.AddField("token", _authToken);
            form.AddField("game_user_id", _userID);
            GameObject newStream = new GameObject("Stream");
            Downloader newDownloader = newStream.AddComponent<Downloader>();
            newStream.transform.parent = transform;
            newDownloader.loadURL(url, form, returnType, callback);
        }
        else
        {
            Hashtable returnData = new Hashtable();
            returnData["error"] = "Return type " + returnType + " doesn't exist.";
            callback(returnData);
        }
    }

    /*
        Function: GoOffline 

        Puts the Web Module in Offline Mode
    */
    public void GoOffline()
    {
        _online = false;
        if (offline == null)
            offline = new Offline();
    }

    /*
        Function: postOfflineAction 

        Handles actions if module is offline.

        Parameters:

        action - The desired action
        form - The WWWForm to submit to the server
        returnType - The type of data that needs to be returned
        callback - A function pointer to the method that will manage the returned data
	
        See Also:
	
        - <GoOffline>
        - <PostAction(WebActions, WWWForm, ReturnTypes, function)>
    */
    private void postOfflineAction(WebActions action, WWWForm form, ReturnTypes returnType, Action<Hashtable> callback)
    {
        if (offline == null)
            offline = new Offline();

        switch (action)
        {
            case WebActions.GetQuestions:
                TextAsset questionsXML = (TextAsset)Resources.Load(Assets.Resources.Data.Questions);
                Hashtable data = GetQuestionDataFromXML(questionsXML.text);
                callback(data);
                break;
            default:
                Hashtable temp = new Hashtable();
                temp["data"] = true;
                callback(temp);
                break;
        }
    }

    // TODO: HACK
    private Hashtable GetQuestionDataFromXML(string xml)
    {
        Hashtable returnData = new Hashtable();
        XMLNode node;
        XMLReader parser = new XMLReader();
        node = parser.read(xml);

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

        return returnData;
    }

    /*************************************************************************/
    /*  Stat Tracking Code */
    /*************************************************************************/



    private DateTime defaultTime = new System.DateTime(0);

    public TimeInMenu curMenu;
    public string curScene;
    public bool trackingScene;
    private System.DateTime curMenuStart;
    private System.DateTime curSceneStart;

    private Hashtable Stats;

    /*
        Function: initStatTracking 

        Initializes statistics tracking variables. Called by <StartSceneTrack> if needed.
	
        See Also:
	
        - <StartSceneTrack>
    */
    private void initStatTracking()
    {
        if (Stats == null)
        {
            Stats = new Hashtable();
            trackingScene = false;
            curMenu = (TimeInMenu)(-1);
            curScene = "";
        }
    }

    /*
        Function: addStat(string, Value) 

        Adds a statistic to the Stats table

        Parameters:

        StatName - Identifier for the statistic to track
        Value - Value for the statistic
	
        See Also:
	
        - <PostStats>
    */
    void addStat(string StatName, object Value)
    {
        StartSceneTrack();
        ((Hashtable)Stats[curScene])[StatName] = Value;
    }

    /*
        Function: addStat(string, Value, bool) 

         Adds a statistic to the Stats table. More functionality than addStat(string, Value)

        Parameters:

        StatName - Identifier for the statistic to track
        Value - Value for the statistic
        Override - If statistic is already in the stats table, it will be copied over
	
        See Also:
	
        - <PostStats>
    */
    void addStat(string StatName, object Value, bool Override)
    {
        if (!Override)
        {
            if (((Hashtable)Stats[curScene])[StatName] != null)
            {
                bool saved = false;
                int iteration = 1;
                while (!saved)
                {
                    if (((Hashtable)Stats[curScene])[StatName + iteration.ToString()] != null)
                    {
                        saved = true;
                        addStat(StatName + iteration.ToString(), Value);
                    }
                }
            }
            else
            {
                addStat(StatName, Value);
            }
        }
        else
        {
            addStat(StatName, Value);
        }
    }

    /*
        Function: TrackMenu 

        Starts tracking the time spent in the current menu

        Parameters:

        newMenu - The menu to be tracked
	
        See Also:
	
        - <EndMenuTrack>
    */
    void TrackMenu(TimeInMenu newMenu)
    {
        EndMenuTrack();

        curMenu = newMenu;
        curMenuStart = System.DateTime.Now;
    }

    /*
        Function: EndMenuTrack 

        Stops tracking the current menu. This should be called any time a menu is closed completely. For instance, if you are closing the in-game menu and going back to gameplay. You can just call TrackMenu again if you are switching between menus

        See Also:

        - <TrackMenu>	
    */
    void EndMenuTrack()
    {
        if ((int)curMenu != -1)
        {
            StartSceneTrack();

            System.TimeSpan newTime = new System.TimeSpan(0, 0, 0);

            if (((Hashtable)Stats[curScene])["time_in_" + curMenu.ToString()] != null)
            {
                newTime = (TimeSpan)((Hashtable)Stats[curScene])["time_in_" + curMenu.ToString()];
            }

            newTime.Add(System.DateTime.Now.Subtract(curMenuStart));
            ((Hashtable)Stats[curScene])["time_in_" + curMenu.ToString()] = newTime;

            curMenu = (TimeInMenu)(-1);
        }
    }

    /*
        Function: TrackScene 

        Ends any previous scene tracking and starts tracking the current scene. This should be called at the start of every scene that you want to track in the level script itself
	
        See Also:
	
        - <EndSceneTrack>
    */
    public void TrackScene()
    {
        EndSceneTrack();

        StartSceneTrack();

        if (((Hashtable)Stats[curScene]).ContainsKey("times_entered"))
        {
            ((Hashtable)Stats[curScene])["times_entered"] = (int)((Hashtable)Stats[curScene])["times_entered"] + 1;
        }
        else
        {
            ((Hashtable)Stats[curScene])["times_entered"] = 1;
        }
    }

    /*
        Function: StartSceneTrack 

        Used by <TrackScene> to start tracking the current scene.
	
        See Also:
	
        - <TrackScene>
    */
    private void StartSceneTrack()
    {
        //Checks if the current scene has been tracked and inits a track if it hasn't
        if (Stats == null)
        {
            initStatTracking();
        }

        if (!trackingScene)
        {
            curScene = Application.loadedLevelName;
            curSceneStart = System.DateTime.Now;
        }

        if (Stats[curScene] == null)
        {
            Stats[curScene] = new Hashtable();
            ((Hashtable)Stats[curScene])["level_id"] = Application.loadedLevel;
        }
    }

    /*
        Function: EndSceneTrack 

        Stops tracking the current scene

        See Also:
	
        - <TrackScene>
    */
    public void EndSceneTrack()
    {
        if (curScene != "" && Stats != null && Stats[curScene] != null)
        {
            //Append time
            System.TimeSpan newTime = System.DateTime.Now.Subtract(curSceneStart);

            if (((Hashtable)Stats[curScene])["time_in_scene"] != null)
            {
                newTime = newTime.Add((TimeSpan)((Hashtable)Stats[curScene])["time_in_scene"]);
            }

            ((Hashtable)Stats[curScene])["time_in_scene"] = newTime;

            curScene = "";
            trackingScene = false;
        }
    }

    /*
        Function: PostStats 

        Submits the Stats table to the server

        See Also:
	
        - <addStat(string, Value)>
        - <addStat(string, Value, bool) >
    */
    public void PostStats()
    {
        EndSceneTrack();  //Finish up the current level time

        WWWForm form;
        int attrIndex;

        foreach (DictionaryEntry trackedLevel in Stats)  //For each level tracked
        {
            attrIndex = 0;
            form = new WWWForm();

            foreach (DictionaryEntry statistic in (Hashtable)trackedLevel.Value)
            {
                //Debug.Log(statistic.Key);
                //Debug.Log(statistic.Value);
                if (((string)statistic.Key) == "level_id")
                {
                    form.AddField("statistic[level_id]", (int)statistic.Value);
                }
                else
                {
                    form.AddField("statistic[detailed_statistics_attributes][" + attrIndex.ToString() + "][key]", statistic.Key.ToString());
                    form.AddField("statistic[detailed_statistics_attributes][" + attrIndex.ToString() + "][value]", statistic.Value.ToString());
                    //Debug.Log(attrIndex);
                    attrIndex++;
                }
            }
            PostAction(WebActions.PostStats, form);
        }
    }

    void OnApplicationQuit()
    {
        PostStats();
    }
}