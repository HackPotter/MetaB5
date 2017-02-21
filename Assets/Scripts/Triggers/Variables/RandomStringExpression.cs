using UnityEngine;

[Expression(EvaluationType = typeof(string), IsDynamicType = false)]
public class RandomStringExpression : LiteralExpression
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string[] _strings;
#pragma warning restore 0067, 0649

    public override object Value
    {
        get
        {
            return _strings[UnityEngine.Random.Range(0, _strings.Length)];
        }
    }
}

