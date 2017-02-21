using UnityEngine;

[Trigger(DisplayPath = "Data")]
[AddComponentMenu("Metablast/Triggers/Actions/Data/Write Save Data to File")]
public class WriteSaveData : EventResponder
{
    public override void OnEvent(ExecutionContext context)
    {
        GameContext.Instance.Player.PersistentStorage.WriteData();
    }
}