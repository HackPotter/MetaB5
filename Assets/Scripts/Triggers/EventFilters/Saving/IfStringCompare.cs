using UnityEngine;


public enum SaveDataType
{
    SaveFile,
    SessionData
}

public enum StringComparisonOperator
{
    Equal,
    Inequal,
    ContainsExpected,
    ExpectedContains,
}

[Trigger(Description = "Compares a string value saved in session storage with the given expected value, and invokes the actions underneath if they are equal.")]
[AddComponentMenu("Metablast/Triggers/Filters/If String Equals")]
public class IfStringCompare : EventFilter
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The key of the string value in session storage that will be compared to the expected value.")]
    private string _key;

    [SerializeField]
    [Infobox("If the value found with the given key is equal to this value, then the actions underneath will be invoked.")]
    private string _expectedValue;

    [SerializeField]
    [Infobox("The operator used to compare the two string values.")]
    private StringComparisonOperator _comparisonOperator;
    
    [SerializeField]
    [Infobox("Whether to look for the given key in the game's current session data, or in the game's save file.")]
    private SaveDataType _dataType = SaveDataType.SessionData;
#pragma warning restore 0067, 0649
    public override void OnEvent(ExecutionContext context)
    {
        string value;
        if (_dataType == SaveDataType.SaveFile)
            value = GameContext.Instance.Player.PersistentStorage.RecallString(_key);
        else
            value = GameContext.Instance.Player.SessionStorage.RecallString(_key);

        if (value != null)
        {
            switch (_comparisonOperator)
            {
            case StringComparisonOperator.Equal:
                if (value.Equals(_expectedValue))
                    TriggerEvent();
                break;
            case StringComparisonOperator.Inequal:
                if (!value.Equals(_expectedValue))
                    TriggerEvent();
                break;
            case StringComparisonOperator.ContainsExpected:
                if (value.Contains(_expectedValue))
                    TriggerEvent();
                break;
            case StringComparisonOperator.ExpectedContains:
                if (_expectedValue.Contains(value))
                    TriggerEvent();
                break;
            }
        }
    }
}

