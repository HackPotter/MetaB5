using UnityEngine;

public class PlayerScoreCompare : EventFilter
{
    private enum Comparison
    {
        LessThan,
        Equal,
        GreaterThan,
    }

#pragma warning disable 0067, 0649
    [SerializeField]
    private Comparison _comparison;
    [SerializeField]
    private int _value;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        switch (_comparison)
        {
            case Comparison.Equal:
                if (_value == GameContext.Instance.Player.Points)
                {
                    TriggerEvent(context);
                }
                break;
            case Comparison.GreaterThan:
                if (_value > GameContext.Instance.Player.Points)
                {
                    TriggerEvent(context);
                }
                break;
            case Comparison.LessThan:
                if (_value < GameContext.Instance.Player.Points)
                {
                    TriggerEvent(context);
                }
                break;
        }
    }
}

