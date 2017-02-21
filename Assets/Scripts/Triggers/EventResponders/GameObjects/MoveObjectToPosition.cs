using UnityEngine;

[Trigger(Description = "Moves the first object to the location of the second object.", DisplayPath="GameObject")]
public class MoveObjectToPosition : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(GameObject), "Object to move")]
    private Expression _objectToMove;

    [SerializeField]
    [ExpressionField(typeof(GameObject), "New Location")]
    private Expression _targetPosition;
#pragma warning restore 0067, 0649

    protected override void Awake()
    {
        base.Awake();

        if (!_objectToMove)
        {
            DebugFormatter.LogError(this, "Object to move must not be null");
            this.enabled = false;
        }

        if (!_targetPosition)
        {
            DebugFormatter.LogError(this, "Target position must not be null");
            this.enabled = false;
        }
    }

    public override void OnEvent(ExecutionContext context)
    {
        GameObject objectToMove = context.Evaluate<GameObject>(_objectToMove);
        GameObject targetPosition = context.Evaluate<GameObject>(_targetPosition);

        objectToMove.transform.position = targetPosition.transform.position;
        objectToMove.transform.rotation = targetPosition.transform.rotation;
    }
}

