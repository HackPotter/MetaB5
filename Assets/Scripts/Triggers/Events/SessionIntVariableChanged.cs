using UnityEngine;

[AddComponentMenu("Metablast/Triggers/Events/Data/Session Int Variable Changed")]
public class SessionIntVariableChanged : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _variableName;

    [SerializeField]
    private int _expectedValue;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        GameContext.Instance.Player.SessionStorage.OnIntVariableStored += SessionStorage_OnIntVariableStored;
    }

    void OnDestroy()
    {
        GameContext.Instance.Player.SessionStorage.OnIntVariableStored -= SessionStorage_OnIntVariableStored;
    }

    void SessionStorage_OnIntVariableStored(string key, int value)
    {
        if (key == _variableName && value == _expectedValue)
        {
            TriggerEvent();
        }
    }
}

