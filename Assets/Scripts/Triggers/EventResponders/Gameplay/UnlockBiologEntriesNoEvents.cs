using UnityEngine;

public class UnlockBiologEntriesNoEvents : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string[] _biologEntriesToUnlock;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        foreach (string entryName in _biologEntriesToUnlock)
        {
            GameContext.Instance.Player.BiologProgress.UnlockEntryQuiet(entryName);
        }
    }
}

