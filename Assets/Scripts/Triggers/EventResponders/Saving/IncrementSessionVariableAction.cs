using System.Collections.Generic;
using UnityEngine;

[Trigger(DisplayPath = "OBSOLETE")]
[AddComponentMenu("Metablast/Triggers/Actions/Data/Increment Session Variable")]
public class IncrementSessionVariableAction : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _key;

    [SerializeField]
    private int _incrementAmount;

    [SerializeField]
    private int _defaultValue;

    
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
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

