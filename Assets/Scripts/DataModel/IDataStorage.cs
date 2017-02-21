using System.Collections.Generic;

public delegate void IntVariableStored(string key, int value);
public delegate void StringVariableStored(string key, string value);
public delegate void FloatVariableStored(string key, float value);

public interface IDataStorage : IEnumerable<KeyValuePair<string,object>>
{
    event IntVariableStored OnIntVariableStored;
    event StringVariableStored OnStringVariableStored;
    event FloatVariableStored OnFloatVariableStored;

    bool HasKeyForString(string key);
    bool HasKeyForInt(string key);
    bool HasKeyForFloat(string key);

    void Store(string key, string value);
    void Store(string key, int value);
    void Store(string key, float value);

    string RecallString(string key);
    int RecallInt(string key);
    float RecallFloat(string key);
    void Remove(string key);
}

