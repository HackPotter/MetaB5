using System.Collections.Generic;
using UnityEngine;

[Expression(EvaluationType = typeof(List<string>), IsDynamicType = false)]
public class StringListLiteralExpression : LiteralExpression
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private List<string> _values;
#pragma warning restore 0067, 0649

    public override object Value
    {
        get { return _values; }
    }
}

