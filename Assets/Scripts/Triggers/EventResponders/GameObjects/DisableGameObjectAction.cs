using UnityEngine;

[Trigger(Description = "Disables the given GameObject.", DisplayPath = "GameObject")]
[AddComponentMenu("Metablast/Triggers/Actions/GameObjects/Disable Game Object")]
public class DisableGameObjectAction : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(GameObject), "GameObject to disable")]
    private Expression _gameObject;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameObject gameObject = context.Evaluate<GameObject>(_gameObject);
        if (!gameObject)
        {
            DebugFormatter.LogError(this, "Expecting GameObject, but Expression evaluated to null.");
            return;
        }
        gameObject.SetActive(false);
    }
}
