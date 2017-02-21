// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
    Class: MegaTimer
    Can instantiate and manage multiple <Timer> objects at once. Inherits from Unity <MonoBehaviour at http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour>
*/
public class MegaTimer : MonoBehaviour
{

    /*
        Function: AddTimer(float, int)

        Initializes a new <Timer>

        Parameters:

        interval - The interval in seconds between timer ticks
        count - The total number of timer ticks

        See Also:

        - <AddTimer(float, int, Boo.Lang.Hash)>
    */
    public void AddTimer(float interval, int count)
    {
        Timer newTimer = addTimerObject();
        newTimer.StartTimer(interval, count, true);
    }

    /*
        Function: AddTimer(float, int, Boo.Lang.Hash)

        Initializes a new <Timer>

        Parameters:

        interval - The interval in seconds between timer ticks
        count - The total number of timer ticks
        callbacks - A Boo.Lang.Hash table containing the "OnTick" and "OnComplete" function pointers

        See Also:

        - <AddTimer(float, int)>
    */
    public void AddTimer(float interval, int count, Dictionary<string, Action> callbacks)
    {
        Timer newTimer = addTimerObject();
        if (callbacks.ContainsKey("OnTick"))
            newTimer.OnTick = callbacks["OnTick"];

        if (callbacks.ContainsKey("OnComplete"))
            newTimer.OnComplete = callbacks["OnComplete"];

        newTimer.StartTimer(interval, count, true);
    }

    public void AddTimer(float interval, int count, Dictionary<string, Action> callbacks, string name)
    {
        Timer newTimer = addTimerObject(name);
        if (callbacks.ContainsKey("OnTick"))
            newTimer.OnTick = callbacks["OnTick"];

        if (callbacks.ContainsKey("OnComplete"))
            newTimer.OnComplete = callbacks["OnComplete"];

        newTimer.StartTimer(interval, count, true);
    }

    /*
        Function: addTimerObject

        Initializes a new <Timer> object and attaches it as a child to this object

        Returns:
	
        The new <Timer> object
	
        See Also:

        - <AddTimer(float, int)>
        - <AddTimer(float, int, Boo.Lang.Hash)>
    */
    private Timer addTimerObject()
    {
        GameObject newTimerGO = new GameObject("Timer");
        Timer newTimer = newTimerGO.AddComponent<Timer>();
        newTimerGO.transform.parent = transform;
        DontDestroyOnLoad(newTimerGO);
        return newTimer;
    }

    private Timer addTimerObject(string name)
    {
        GameObject newTimerGO = new GameObject(name);
        Timer newTimer = newTimerGO.AddComponent<Timer>();
        newTimerGO.transform.parent = transform;
        DontDestroyOnLoad(newTimerGO);
        return newTimer;
    }

    /*
        Function: StopAll

        Stops all <Timer> objects attached to <MegaTimer>
	
        See Also:

        - <KillAll>
    */
    void StopAll()
    {
        Timer[] Timers = GetComponentsInChildren<Timer>();
        foreach (Timer temp in Timers)
        {
            temp.StopTimer();
        }
    }

    /*
        Function: KillAll

        Destroys all <Timer> objects attached to <MegaTimer>
	
        See Also:

        - <StopAll>
    */
    void KillAll()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}