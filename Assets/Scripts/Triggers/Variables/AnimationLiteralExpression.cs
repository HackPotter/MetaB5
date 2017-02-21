using UnityEngine;

[Expression(EvaluationType = typeof(Animation), IsDynamicType = false)]
public class AnimationLiteralExpression : LiteralExpression
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private Animation _animation;
#pragma warning restore 0067, 0649

    public override object Value
    {
        get { return _animation; }
    }
}

