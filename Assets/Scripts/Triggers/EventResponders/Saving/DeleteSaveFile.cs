using UnityEngine;

[Trigger(DisplayPath = "Data",
    Description = "Deletes the player's persistent save file and clears the data.")]
public class DeleteSaveFile : EventResponder
{
    public override void OnEvent(ExecutionContext context)
    {
        GameContext.Instance.Player.PersistentStorage.ClearData();
		GameContext.Instance.Player.CurrentObjectives.Clear();
    }
}

