//using System.Collections.Generic;
//using UnityEngine;

//[Trigger(DisplayPath = "OBSOLETE")]
//[AddComponentMenu("Metablast/Triggers/Actions/Data/Save Temporary Data")]
//public class SaveTemporaryDataAction : EventResponder
//{
//#pragma warning disable 0067, 0649
//    [SerializeField]
//    private string _key;

//    [SerializeField]
//    private string _value;
//#pragma warning restore 0067, 0649

//    public override void OnEvent(ExecutionContext context)
//    {
//        IDataStorage data = GameContext.Instance.Player.SessionStorage;
//        data.Store(_key, _value);
//    }
//}

