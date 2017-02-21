using UnityEngine;

[Trigger(Description = "Enables the given GameObject.", DisplayPath = "GameObject")]
[AddComponentMenu("Metablast/Triggers/Actions/GameObjects/Enable Game Object")]
public class EnableGameObject : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(GameObject), "GameObject to enable")]
    private Expression _gameObject;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameObject gameObject = context.Evaluate<GameObject>(_gameObject);
        if (!gameObject)
        {
            DebugFormatter.LogError(this, "Expected GameObject, but expression evaluated to null.");
            return;
        }
        gameObject.SetActive(true);
    }
}
