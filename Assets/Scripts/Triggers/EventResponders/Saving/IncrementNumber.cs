using System.Collections.Generic;
using UnityEngine;

[Trigger(DisplayPath = "Data",
    Description ="Increments the value of an integer stored at the given key in either session data or player save file. If the provided key does not exist, it will be created and assigned the given default value before incrementing.")]
[AddComponentMenu("Metablast/Triggers/Actions/Data/Increment Int Variable")]
public class IncrementNumber : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The key used to retrieve an integer value from storage.")]
    private string _key;

    [SerializeField]
    [Infobox("The amount to increment the value by. For example, if the given value is 2 and Increment Amount is 3, then the value will be 5 after the first time the action is invoked, and 8 after the second.")]
    private int _incrementAmount;

    [SerializeField]
    [Infobox("The default value to store before incrementing if the given key is not found.")]
    private int _defaultValue;

    [SerializeField]
    [Infobox("Whether to look for the given key in the game's current session data, or in the game's save file.")]
    private SaveDataType _saveDataType  = SaveDataType.SessionData;

    [SerializeField]
    [Infobox("If checked and Save Data Type is Save File, the modified save file will be written to disk immediately.")]
    private bool _writeImmediately;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_saveDataType == SaveDataType.SaveFile)
        {
            IPersistentDataStorage data = GameContext.Instance.Player.PersistentStorage;
            if (!data.HasKeyForInt(_key))
            {
                data.Store(_key, _defaultValue);
            }

            int value = data.RecallInt(_key);
            data.Store(_key, value + _incrementAmount);

            if (_writeImmediately)
            {
                data.WriteData();
            }
        }
        else
        {
            IDataStorage data = GameContext.Instance.Player.SessionStorage;
            if (!data.HasKeyForInt(_key))
            {
                data.Store(_key, _defaultValue);
            }

            int value = data.RecallInt(_key);
            data.Store(_key, value + _incrementAmount);
        }

    }
}

