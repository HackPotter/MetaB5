using UnityEngine;

[Trigger(DisplayPath = "Data")]
[AddComponentMenu("Metablast/Triggers/Actions/Data/Store Save File String")]
public class StoreString : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The key that will be used to save the given string value. The key allows you to recall the data later.")]
    private string _key;

    [SerializeField]
    [Infobox("The value of the string to store.")]
    private string _value;

    [SerializeField]
    [Infobox("Whether to look for the given key in the game's current session data, or in the game's save file.")]
    private SaveDataType _saveDataType = SaveDataType.SessionData;

    [SerializeField]
    [Infobox("If checked and Save Data Type is Save File, the modified save file will be written to disk immediately.")]
    private bool _writeImmediately;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_saveDataType == SaveDataType.SaveFile)
        {
            GameContext.Instance.Player.PersistentStorage.Store(_key, _value);
            if (_writeImmediately)
            {
                GameContext.Instance.Player.PersistentStorage.WriteData();
            }
        }
        else
        {
            GameContext.Instance.Player.SessionStorage.Store(_key, _value);
        }
    }
}

