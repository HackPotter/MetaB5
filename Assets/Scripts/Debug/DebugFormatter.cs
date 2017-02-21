using UnityEngine;

public static class DebugFormatter
{
    public static void Log(Component sender, string message)
    {
        Debug.Log(string.Format("{0} - {1}: {2}", sender.name, sender.GetType().Name, message),sender);
    }

    public static void LogWarning(object sender, string message)
    {
        Debug.LogWarning(string.Format("{0}: {1}", sender.GetType().Name, message));
    }

    public static void LogWarning(object sender, string message, params object[] parameters)
    {
        Debug.LogWarning(string.Format("{0}: {1}", sender.GetType().Name, string.Format(message, parameters)));
    }

    public static void LogWarning(Object sender, string message)
    {
        Debug.LogWarning(string.Format("{0} - {1}: {2}", sender.name, sender.GetType().Name, message), sender);
    }

    public static void LogWarning(Object sender, string message, params object[] parameters)
    {
        Debug.LogWarning(string.Format("{0} - {1}: {2}", sender.name, sender.GetType().Name, string.Format(message, parameters)), sender);
    }

    public static void LogError(object sender, string message)
    {
        Debug.LogError(string.Format("{0}: {1}", sender.GetType().Name, message));
    }

    public static void LogError(object sender, string message, params object[] parameters)
    {
        Debug.LogError(string.Format("{0}: {1}", sender.GetType().Name, string.Format(message, parameters)));
    }

    public static void LogError(Object sender, string message)
    {
        Debug.LogError(string.Format("{0} - {1}: {2}", sender.name, sender.GetType().Name, message), sender);
    }

    public static void LogError(Object sender, string message, params object[] parameters)
    {
        Debug.LogError(string.Format("{0} - {1}: {2}", sender.name, sender.GetType().Name, string.Format(message, parameters)), sender);
    }
}

