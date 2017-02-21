using UnityEngine;

[Trigger(DisplayPath = "Deprecated",
    Description = "Compares a string value saved in session storage with the given expected value, and invokes the actions underneath if they are not equal.")]
public class IfStringNotEqual : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The key of the string value in storage that will be compared to the expected value.")]
    private string _key;

    [SerializeField]
    [Infobox("If the value found with the given key is NOT equal to this value, then the actions underneath will be invoked.")]
    private string _expectedValue;

    [SerializeField]
    [Infobox("Whether to look for the given key in the game's current session data, or in the game's save file.")]
    private SaveDataType _dataType = SaveDataType.SessionData;
#pragma warning restore 0067, 0649
    public override void OnEvent(ExecutionContext context)
    {
        object value;
        if (_dataType == SaveDataType.SaveFile)
            value = GameContext.Instance.Player.PersistentStorage.RecallString(_key);
        else
            value = GameContext.Instance.Player.SessionStorage.RecallString(_key);
        if (value == null || !value.Equals(_expectedValue))
        {
            TriggerEvent(context);
        }
    }
}

