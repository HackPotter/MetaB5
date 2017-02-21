using UnityEngine;


public enum IntegerComparisonOperator
{
    Greater,
    LessThan,
    Equals,
    GreaterEqual,
    LessEqual
}

[Trigger(Description = "Receives an integer stored in the session data with the given key and compares it to the given expected value")]
public class IfIntegerCompare : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The key used to retrieve an integer value from session storage.")]
    private string _key;

    [SerializeField]
    [Infobox("The comparison value to use.")]
    private int _comparisonValue;

    [SerializeField]
    [Infobox("The operator that will be used to compare the values.")]
    private IntegerComparisonOperator _comparisonOperator;

    [SerializeField]
    [Infobox("Whether to look for the given key in the game's current session data, or in the game's save file.")]
    private SaveDataType _dataType = SaveDataType.SessionData;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        int val;
        if (_dataType == SaveDataType.SaveFile)
            val = GameContext.Instance.Player.PersistentStorage.RecallInt(_key);
        else
            val = GameContext.Instance.Player.SessionStorage.RecallInt(_key);


        switch (_comparisonOperator)
        {
        case IntegerComparisonOperator.Equals:
            if (val == _comparisonValue)
                TriggerEvent();
                break;
        case IntegerComparisonOperator.Greater:
            if (val > _comparisonValue)
                TriggerEvent();
            break;
        case IntegerComparisonOperator.GreaterEqual:
            if (val >= _comparisonValue)
                TriggerEvent();
            break;
        case IntegerComparisonOperator.LessThan:
            if (val < _comparisonValue)
                TriggerEvent();
            break;
        case IntegerComparisonOperator.LessEqual:
            if (val <= _comparisonValue)
                TriggerEvent();
            break;
        }
    }
}

