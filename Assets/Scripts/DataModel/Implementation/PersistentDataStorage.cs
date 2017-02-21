using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class PersistentDataStorage : IPersistentDataStorage
{
    private string _fileName;
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public PersistentDataStorage(string fileName)
    {
        _fileName = fileName;
    }

    public void ClearData()
    {
        _data.Clear();
        try
        {
            File.Delete(_fileName);
        }
        catch (Exception) { }
    }

    public void Store(string key, string value)
    {
        _data[key] = value;

        if (OnStringVariableStored != null)
        {
            OnStringVariableStored(key, value);
        }

        WriteData();
    }

    public void Store(string key, int value)
    {
        _data[key] = value;

        if (OnIntVariableStored != null)
        {
            OnIntVariableStored(key, value);
        }

        WriteData();
    }

    public void Store(string key, float value)
    {
        _data[key] = value;

        if (OnFloatVariableStored != null)
        {
            OnFloatVariableStored(key, value);
        }

        WriteData();
    }

    public string RecallString(string key)
    {
        object value;
        if (_data.TryGetValue(key, out value))
        {
            if (value is string)
            {
                return value as string;
            }
            else
            {
                DebugFormatter.LogError(this, "Attempted to recall string with key '{0}', but value was of type {1}", key, value.GetType());
                return null;
            }
        }
        else
        {
            //DebugFormatter.LogError(this, "Attempted to recall string with key '{0}', but value was not found.", key);
            return null;
        }
    }

    public int RecallInt(string key)
    {
        object value;
        if (_data.TryGetValue(key, out value))
        {
            if (value is int)
            {
                return (int)value;
            }
            else
            {
                DebugFormatter.LogError(this, "Attempted to recall string with key '{0}', but value was of type {1}", key, value.GetType());
                return -1;
            }
        }
        else
        {
            DebugFormatter.LogError(this, "Attempted to recall string with key '{0}', but value was not found.", key);
            return -1;
        }
    }

    public float RecallFloat(string key)
    {
        object value;
        if (_data.TryGetValue(key, out value))
        {
            if (value is float)
            {
                return (float)value;
            }
            else
            {
                DebugFormatter.LogError(this, "Attempted to recall string with key '{0}', but value was of type {1}", key, value.GetType());
                return -1;
            }
        }
        else
        {
            DebugFormatter.LogError(this, "Attempted to recall string with key '{0}', but value was not found.", key);
            return -1;
        }
    }

    public void Remove(string key)
    {
        _data.Remove(key);
    }

    public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, object>> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    public void ReadData()
    {
        _data.Clear();
        Debug.Log("Reading Data");
        using (JsonTextReader reader = new JsonTextReader(new StreamReader(File.Open(_fileName, FileMode.OpenOrCreate))))
        {
            JsonSerializer serializer = new JsonSerializer();
            _data = serializer.Deserialize<Dictionary<string, object>>(reader);
        }
        Debug.Log("Finished Reading Data.");

        Debug.Log("Read data from file. Found: ");

        if (_data == null)
        {
            Debug.Log("Data failed to deserialize.");
        }
        else
            foreach (var kvp in _data)
            {
                Debug.Log("{ " + kvp.Key + ", " + kvp.Value.ToString() + " }");
            }

        if (_data == null)
        {
            _data = new Dictionary<string, object>();
        }

        //using (StreamReader reader = new StreamReader(File.Open(_fileName, FileMode.OpenOrCreate)))
        //{
        //    while (!reader.EndOfStream)
        //    {
        //        string line1 = reader.ReadLine();
        //        string line2 = reader.ReadLine();
        //        string line3 = reader.ReadLine();

        //        switch (line1)
        //        {
        //        case "f":
        //            float value;
        //            if (float.TryParse(line3, out value))
        //            {
        //                _data.Add(line2, value);
        //            }
        //            else
        //            {
        //                DebugFormatter.LogError(this, "Error parsing value. Expected float, but was unable to parse {0}", line3);
        //            }
        //            break;
        //        case "i":
        //            int intValue;
        //            if (int.TryParse(line3, out intValue))
        //            {
        //                _data.Add(line2, intValue);
        //            }
        //            else
        //            {
        //                DebugFormatter.LogError(this, "Error parsing value. Expected float, but was unable to parse {0}", line3);
        //            }
        //            break;
        //        case "s":
        //            _data.Add(line2, line3);
        //            break;
        //        }
        //    }
        //}
    }

    public void WriteData()
    {
        using (StreamWriter writer = new StreamWriter(File.Create(_fileName)))
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Serialize(writer, _data);
        }

        //using (StreamWriter writer = new StreamWriter(File.Create(_fileName)))
        //{
        //    foreach (var dataEntry in _data)
        //    {
        //        if (dataEntry.Value is int)
        //        {
        //            writer.WriteLine("i");
        //            writer.WriteLine(dataEntry.Key);
        //            writer.WriteLine((int)dataEntry.Value);
        //        }
        //        else if (dataEntry.Value is float)
        //        {
        //            writer.WriteLine("f");
        //            writer.WriteLine(dataEntry.Key);
        //            writer.WriteLine((float)dataEntry.Value);
        //        }
        //        else if (dataEntry.Value is string)
        //        {
        //            writer.WriteLine("s");
        //            writer.WriteLine(dataEntry.Key);
        //            writer.WriteLine((string)dataEntry.Value);
        //        }
        //        else
        //        {
        //            DebugFormatter.LogError(this, "Unsupported object type {0}", dataEntry.Value.GetType().Name);
        //        }
        //    }
        //}
    }

    public bool HasKeyForString(string key)
    {
        return _data.ContainsKey(key) && _data[key] is string;
    }

    public bool HasKeyForInt(string key)
    {
        return _data.ContainsKey(key) && _data[key] is int;
    }

    public bool HasKeyForFloat(string key)
    {
        return _data.ContainsKey(key) && _data[key] is float;
    }

    public event IntVariableStored OnIntVariableStored;

    public event StringVariableStored OnStringVariableStored;

    public event FloatVariableStored OnFloatVariableStored;
}

