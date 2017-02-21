using UnityEngine;

[Trigger(Description = "DEPRECATED: Use MoveObjectToPosition instead.", DisplayPath = "Deprecated")]
public class DeprecatedMoveObjectToPosition : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [ExpressionField(typeof(GameObject), "Object to move")]
    private Expression _objectToMove;
    [SerializeField]
    private Transform _targetPosition;
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

        objectToMove.transform.position = _targetPosition.transform.position;
        objectToMove.transform.rotation = _targetPosition.transform.rotation;
    }
}

