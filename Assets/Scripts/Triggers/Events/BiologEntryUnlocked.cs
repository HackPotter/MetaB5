
[Trigger(Description = "Invoked when the player unlocks a Biolog entry.", DisplayPath = "Biolog")]
public class BiologEntryUnlocked : EventSender
{
    protected override void OnStart()
    {
        base.OnStart();
        GameContext.Instance.Player.BiologProgress.BiologEntryUnlocked += BiologProgress_BiologEntryUnlocked;
    }

    void OnDestroy()
    {
        GameContext.Instance.Player.BiologProgress.BiologEntryUnlocked -= BiologProgress_BiologEntryUnlocked;
    }

    void BiologProgress_BiologEntryUnlocked(BiologEntry unlockedEntry, bool notify)
    {
        if (notify)
        {
            TriggerEvent();
        }
    }
}

