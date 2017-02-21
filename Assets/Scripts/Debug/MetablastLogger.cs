using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MetablastLogger
{
    private const string kLogFile = "MetablastLog.txt";
    private const string kLogDirectory = "Log/";
    private static MetablastLogger _instance;
    public static MetablastLogger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MetablastLogger();
            }
            return _instance;
        }
    }

    public MetablastLogger()
    {
        /*
        if (!Directory.Exists(kLogDirectory))
        {
            Directory.CreateDirectory(kLogDirectory);
        }
        string filePath = Path.Combine(kLogDirectory, kLogFile);
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }*/
        //_fileWriter = new StreamWriter(File.Open(Path.Combine(kLogDirectory,kLogFile), FileMode.Append));
    }

    public void Start()
    {
        //_fileWriter = new StreamWriter(File.Open(Path.Combine(kLogDirectory, kLogFile), FileMode.Append));
    }

    public void End()
    {
        //_fileWriter.Close();
    }

    public void LogMessage(UnityEngine.Object sender, string message, params object[] parameters)
    {
        //_fileWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", 2, DateTime.Now, sender.name, string.Format(message, parameters)));
    }

    public void LogMessage(object sender, string message, params object[] parameters)
    {
        //_fileWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", 2, DateTime.Now, sender.GetType().Name, string.Format(message, parameters)));
    }

    public void LogWarning(UnityEngine.Object sender, string message, params object[] parameters)
    {
        //_fileWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", 1, DateTime.Now, sender.name, string.Format(message, parameters)));
    }

    public void LogWarning(object sender, string message, params object[] parameters)
    {
        //_fileWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", 1, DateTime.Now, sender.GetType().Name, string.Format(message, parameters)));
    }

    public void LogError(UnityEngine.Object sender, string message, params object[] parameters)
    {
        //_fileWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", 0, DateTime.Now, sender.name, string.Format(message, parameters)));
    }

    public void LogError(object sender, string message, params object[] parameters)
    {
        //_fileWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", 0, DateTime.Now, sender.GetType().Name, string.Format(message, parameters)));
    }
}

