// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

/*
	Class: Timer
	A Timer with OnTick and OnComplete events. Inherits from Unity <MonoBehaviour at http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour>
*/
public class Timer : MonoBehaviour
{
    private int _total_count = 0;
    private long _current_count = 0;
    private float _interval = 0.0f;
    private float _elapsed_time = 0.0f;
    private bool _timer_running = false;
    private bool _destroy_on_complete = false;

    // Variable: OnTick
    // Function pointer to the OnTick event handler
    public Action OnTick;

    // Variable: OnComplete
    // Function pointer to the OnComplete event handler
    public Action OnComplete;

    void Update()
    {
        if (_timer_running)
        {
            _elapsed_time += Time.deltaTime;
            //if elapsed time is greater than or equal to the interval...
            //AND if total count is greater than 0

            if (_elapsed_time >= _interval && (_total_count > 0 ? _current_count < _total_count : true))
            {
                ++_current_count;
                _elapsed_time = 0;
                if (OnTick != null)
                    OnTick();
            }
            else if (_total_count > 0 ? _current_count >= _total_count : false)
            {
                _timer_running = false;
                if (OnComplete != null)
                    OnComplete();
                if (_destroy_on_complete)
                    Destroy(gameObject);
            }
        }
    }

    /*
        Function: getInterval
	
        Returns the interval in seconds between timer ticks
    */
    public float getInterval()
    {
        return _interval;
    }

    /*
        Function: setInterval
	
        Sets the interval in seconds between timer ticks
	
        Parameters:
	
        interval - The interval in seconds
    */
    public void setInterval(float interval)
    {
        _interval = interval;
    }

    /*
        Function: StartTimer(float, int)
	
        Starts the timer
	
        Parameters:
	
        interval - The interval in seconds between timer ticks
        count - The total number of timer ticks
	
        See Also:
	
        - <StartTimer(float, int, bool) >
        - <StopTimer>
    */
    public void StartTimer(float interval, int count)
    {
        _interval = interval;
        _total_count = count;
        _timer_running = true;
    }

    /*
        Function: StartTimer(float, int, bool)
	
         Starts the timer
	
        Parameters:
	
        interval - The interval in seconds between timer ticks
        count - The total number of timer ticks
        destroyOnComplete - If true, timer object will be destroyed when the timer completes
	
        See Also:
	
        - <StartTimer(float, int)>
        - <StopTimer>
    */
    public void StartTimer(float interval, int count, bool destroyOnComplete)
    {
        _interval = interval;
        _total_count = count;
        _timer_running = true;
        _destroy_on_complete = destroyOnComplete;
    }

    /*
        Function: StopTimer
	
        Stops the timer
	
        See Also:
	
        - <StartTimer(float, int)>
        - <StartTimer(float, int, bool) >
    */
    public void StopTimer()
    {
        _timer_running = false;
    }

    /*
        Function: ResetTimer
	
        Resets the timer object. If running, the timer will be stopped
    */
    public void ResetTimer()
    {
        _elapsed_time = 0.0f;
        _interval = 0.0f;
        _current_count = 0;
        _total_count = 0;
        _timer_running = false;
    }

    /*
        Function: GetCurrentCount
	
        Returns the number of times the timer has ticked
    */
    public long GetCurrentCount()
    {
        return _current_count;
    }

    /*
        Function: GetTotalCount
	
        Returns the number of ticks needed to complete the timer
    */
    public int GetTotalCount()
    {
        return _total_count;
    }

    /*
        Function: IsTimerRunning
	
        Returns true if the timer is running
    */
    public bool IsTimerRunning()
    {
        return _timer_running;
    }

    /*
        Function: Destroy
	
        Destroys the Timer object (NOT the GameObject)
    */
    public void Destroy()
    {
        Destroy(transform.GetComponent<Timer>());
    }
}
