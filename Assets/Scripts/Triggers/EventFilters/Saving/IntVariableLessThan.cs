using UnityEngine;

[Trigger(DisplayPath = "Deprecated",
    Description = "Receives an integer stored in the session data with the given key and compares it to the given expected value. If the retrieved value is greater than the given expected value, the actions under this filter are invoked.")]
public class IntVariableLessThan : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The key used to retrieve an integer value from session storage.")]
    private string _key;

    [SerializeField]
    private int _expectedValue;

    [SerializeField]
    [Infobox("Whether to look for the given key in the game's current session data, or in the game's save file.")]
    private SaveDataType _dataType = SaveDataType.SessionData;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        int val;
        if (_dataType == SaveDataType.SaveFile)
            val = GameContext.Instance.Player.PersistentStorage.RecallInt(_key);
        else
            val = GameContext.Instance.Player.SessionStorage.RecallInt(_key);

        if (val < _expectedValue)
        {
            TriggerEvent();
        }
    }
}

