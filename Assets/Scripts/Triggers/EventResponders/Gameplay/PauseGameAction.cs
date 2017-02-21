using UnityEngine;

[Trigger(Description = "Pauses the game.", DisplayPath = "Gameplay")]
public class PauseGameAction : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private PauseLevel _PauseLevel;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameState.Instance.PauseLevel = _PauseLevel;
    }
}

