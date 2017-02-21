using UnityEngine;

[Trigger(Description="Destroys the given game object.", DisplayPath = "GameObject")]
[AddComponentMenu("Metablast/Triggers/Actions/GameObjects/Destroy Game Object On Trigger")]
public class DestroyGameObjectOnTrigger : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(GameObject),"GameObject to destroy")]
    private Expression _gameObjectToDestroy;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (!_gameObjectToDestroy)
        {
            DebugFormatter.LogError(this, "Expression must not be null");
            return;
        }

        GameObject gameObjectToDestroy = context.Evaluate<GameObject>(_gameObjectToDestroy);

        if (!gameObjectToDestroy)
        {
            DebugFormatter.LogError(this, "Expecting GameObject, but Expression evaluated to null.");
            return;
        }

        Destroy(gameObjectToDestroy);   
    }
}

