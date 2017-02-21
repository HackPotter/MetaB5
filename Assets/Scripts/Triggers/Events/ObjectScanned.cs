using UnityEngine;

[Trigger(Description = "Invoked when the player has scanned an object with the given biolog entry name.", DisplayPath = "Biolog")]
public class ObjectScanned : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The name of the entry for which this event will be invoked.")]
    private string _entryName;
#pragma warning restore 0067, 0649

    protected override void OnAwake()
    {
        GameContext.Instance.Player.BiologProgress.BiologEntryScanned += BiologProgress_BiologEntryScanned;
    }

    void OnDestroy()
    {
        GameContext.Instance.Player.BiologProgress.BiologEntryScanned -= BiologProgress_BiologEntryScanned;
    }

    void BiologProgress_BiologEntryScanned(BiologEntry unlockedEntry, bool notify)
    {
        if (notify && unlockedEntry.EntryName == _entryName)
        {
            this.TriggerEvent();
        }
    }
}
