using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TransientDataStorage : IPersistentDataStorage
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public void ReadData() { }

    public void WriteData() { }

    public void ClearData()
    {
        _data.Clear();
    }

    public event IntVariableStored OnIntVariableStored;

    public event StringVariableStored OnStringVariableStored;

    public event FloatVariableStored OnFloatVariableStored;

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
                DebugFormatter.LogError(this, "Attempted to recall int with key '{0}', but value was of type {1}", key, value.GetType());
                return -1;
            }
        }
        else
        {
            DebugFormatter.LogError(this, "Attempted to recall int with key '{0}', but value was not found.", key);
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
                DebugFormatter.LogError(this, "Attempted to recall float with key '{0}', but value was of type {1}", key, value.GetType());
                return -1;
            }
        }
        else
        {
            DebugFormatter.LogError(this, "Attempted to recall float with key '{0}', but value was not found.", key);
            return -1;
        }
    }

    public void Remove(string key)
    {
        _data.Remove(key);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }
}

