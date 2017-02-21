using System;
using UnityEngine;

public enum LogEntryType
{
    MouseClick = 0,
    PlayerPositionUpdate = 1,
    SceneLoaded = 2, //done
    BiologOpened = 3, //done
    BiologEntryUnlocked = 4, //done
    BiologEntrySelected = 5, //done
    BiologImageViewed = 6, //nah
    
    // Tools
    ToolEnabled = 7, //done
    LightToolActive = 8, //done

    ObjectScanned = 9, // done
    ObjectGrabbed = 10, //done
    ObjectDropped = 11, //done

    // Questions
    QuestionViewed = 12, //done
    QuestionAnswered = 13, //done

    //Objectives
    ObjectiveAdded = 14, //done
    ObjectiveTaskComplete = 15,//done
    ObjectiveComplete = 16,//done

    //Resources
    ResourceEvent = 17, //done
}

public class AnalyticsLogEntry
{
    public virtual LogEntryType LogEntryType
    {
        get;
        private set;
    }

    public virtual string Data
    {
        get;
        private set;
    }

    public Guid UserGuid
    {
        get;
        private set;
    }


    public float EventTime
    {
        get;
        private set;
    }

    public AnalyticsLogEntry(Guid userGuid)
    {
        UserGuid = userGuid;
        EventTime = Time.realtimeSinceStartup;
    }

    public AnalyticsLogEntry(Guid userGuid, float overrideTime)
    {
        UserGuid = userGuid;
        EventTime = overrideTime;
    }

    public AnalyticsLogEntry(Guid userGuid, LogEntryType logEntryType, string data, float eventTime)
    {
        UserGuid = userGuid;
        LogEntryType = logEntryType;
        Data = data;
        EventTime = eventTime;
    }
}

