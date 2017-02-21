//using UnityEngine;

//[Trigger(DisplayPath = "Data")]
//[AddComponentMenu("Metablast/Triggers/Actions/Data/Store Save File String")]
//public class StoreSaveFileString : EventResponder
//{
//#pragma warning disable 0067, 0649
//    [SerializeField]
//    private string _key;

//    [SerializeField]
//    private string _value;

//    [SerializeField]
//    private SaveDataType _saveDataType;
    
//    [SerializeField]
//    private bool _writeImmediately;
//#pragma warning restore 0067, 0649

//    public override void OnEvent(ExecutionContext context)
//    {
//        if (_saveDataType == SaveDataType.SaveFile)
//        {
//            if (_writeImmediately)
//            {
//                GameContext.Instance.Player.PersistentStorage.Store(_key, _value);
//                if (_writeImmediately)
//                {
//                    GameContext.Instance.Player.PersistentStorage.WriteData();
//                }
//            }
//        }
//        else
//        {
//            IDataStorage data = GameContext.Instance.Player.SessionStorage;
//            data.Store(_key, _value);
//        }
//    }
//}

