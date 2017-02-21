using System.Collections.Generic;
using UnityEngine;

[Trigger(DisplayPath = "Data")]
[AddComponentMenu("Metablast/Triggers/Actions/Data/Remove Data")]
public class RemoveData : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _key;

    [SerializeField]
    private SaveDataType _saveDataType = SaveDataType.SessionData;

    [SerializeField]
    private bool _writeImmediately;
#pragma warning restore 0067, 0649
    public override void OnEvent(ExecutionContext context)
    {
        if (_saveDataType == SaveDataType.SaveFile)
        {
            GameContext.Instance.Player.PersistentStorage.Remove(_key);
            if (_writeImmediately)
            {
                GameContext.Instance.Player.PersistentStorage.WriteData();
            }
        }
        else
        {
            IDataStorage data = GameContext.Instance.Player.SessionStorage;
            data.Remove(_key);
        }
    }
}

