//using UnityEngine;

//[Trigger(DisplayPath = "Data")]
//[AddComponentMenu("Metablast/Triggers/Actions/Data/Store Save File Number")]
//public class StoreSaveFileNumber : EventResponder
//{
//#pragma warning disable 0067, 0649
//    [SerializeField]
//    private string _key;

//    [SerializeField]
//    private int _value;

//    [SerializeField]
//    private bool _writeDataToDisk;
//#pragma warning restore 0067, 0649

//    public override void OnEvent(ExecutionContext context)
//    {
//        IPersistentDataStorage data = GameContext.Instance.Player.PersistentStorage;
//        data.Store(_key, _value);

//        if (_writeDataToDisk)
//        {
//            data.WriteData();
//        }
//    }
//}

