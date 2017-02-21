using System;
using UnityEngine;

public class ApplicationEvents : MonoBehaviour
{
    private static ApplicationEvents _instance;

    public static ApplicationEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject applicationEvents = new GameObject("ApplicationEvents");
                _instance = applicationEvents.AddComponent<ApplicationEvents>();
            }
            return _instance;
        }
    }

    public event Action ApplicationQuit;
    public event Action<bool> ApplicationPaused;
    public event Action<bool> ApplicationFocused;

    void Awake()
    {
        if (_instance != null)
        {
            DebugFormatter.LogError(this, "ApplicationEvents is a singleton, but an instance already exists.");
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void OnApplicationQuit()
    {
        if (ApplicationQuit != null)
        {
            ApplicationQuit();
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (ApplicationFocused != null)
        {
            ApplicationFocused(focusStatus);
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (ApplicationPaused != null)
        {
            ApplicationPaused(pauseStatus);
        }
    }
}

