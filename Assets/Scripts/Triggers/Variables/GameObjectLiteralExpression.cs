using UnityEngine;

[Expression(EvaluationType = typeof(GameObject), IsDynamicType = false)]
public class GameObjectLiteralExpression : LiteralExpression
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject _value;
#pragma warning restore 0067, 0649

    public override object Value
    {
        get { return _value; }
    }
}
