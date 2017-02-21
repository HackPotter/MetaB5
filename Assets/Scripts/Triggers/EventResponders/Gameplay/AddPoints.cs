using UnityEngine;

[Trigger(Description = "Adds points to the player's score.", DisplayPath = "Gameplay")]
public class AddPoints : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private int _value;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameContext.Instance.Player.Points += _value;
    }
}
