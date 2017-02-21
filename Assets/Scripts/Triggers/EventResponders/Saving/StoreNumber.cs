using System.Collections.Generic;
using UnityEngine;

[Trigger(DisplayPath = "Data")]
[AddComponentMenu("Metablast/Triggers/Actions/Data/Store Number")]
public class StoreNumber : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _key;

    [SerializeField]
    private int _value;

    [SerializeField]
    private SaveDataType _saveDataType = SaveDataType.SessionData;

    [SerializeField]
    private bool _writeImmediately;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_saveDataType == SaveDataType.SaveFile)
        {
            if (_writeImmediately)
            {
                GameContext.Instance.Player.PersistentStorage.Store(_key, _value);
                if (_writeImmediately)
                {
                    GameContext.Instance.Player.PersistentStorage.WriteData();
                }
            }
        }
        else
        {
            IDataStorage data = GameContext.Instance.Player.SessionStorage;
            data.Store(_key, _value);
        }
    }
}