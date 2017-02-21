using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Metablast/Triggers/Filters/Session Data Equality Filter")]
[Trigger(Description = "DEPRECATED: Use DataEqualityFilter instead.", DisplayPath = "Deprecated")]
public class SessionDataEqualityFilter : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _key;
    [SerializeField]
    private string _expectedValue;
    [SerializeField]
    [Infobox("Whether to look for the given key in the game's current session data, or in the game's save file.")]
    private SaveDataType _dataType;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        object value = GameContext.Instance.Player.SessionStorage.RecallString(_key);
        if (value != null && value.Equals(_expectedValue))
        {
            TriggerEvent();
        }
    }
}

