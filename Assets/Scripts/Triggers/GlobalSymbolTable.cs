using UnityEngine;
using System.Collections.Generic;

public class GlobalSymbolTable : ScriptableObject
{
    [SerializeField]
    private List<Variable> _variables = new List<Variable>();
    [SerializeField]
    private List<string> _variableIdentifiers = new List<string>();

    private Dictionary<string, Variable> _variablesByIdentifier = new Dictionary<string, Variable>();

    public Dictionary<string, Variable> Variables
    {
        get { return _variablesByIdentifier; }
    }

    public Variable GetVariable(string identifier)
    {
        if (_variablesByIdentifier.ContainsKey(identifier))
        {
            return _variablesByIdentifier[identifier];
        }
        else
        {
            DebugFormatter.LogError(this, "Could not find variable with identifier {0}", identifier);
            return null;
        }
    }

    public void SetVariable(string identifier, object value)
    {
        _variablesByIdentifier[identifier].Value = value;
    }

    public void AddVariable(string identifier, Variable value)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            DebugFormatter.LogError(this, "Variable identifier cannot be null or empty.");
            return;
        }
        if (!value)
        {
            DebugFormatter.LogError(this, "Attempting to add variable with identifier {0}, but Variable is null", identifier);
            return;
        }
        _variableIdentifiers.Add(identifier);
        _variables.Add(value);
        _variablesByIdentifier.Add(identifier, value);
    }

    public void DeleteVariable(string identifier)
    {
        Variable var = _variablesByIdentifier[identifier];
        _variables.Remove(var);
        _variableIdentifiers.Remove(identifier);

        _variablesByIdentifier.Remove(identifier);
    }

    void OnEnable()
    {
        _variablesByIdentifier.Clear();
        for (int i = 0; i < _variables.Count; i++)
        {
            _variablesByIdentifier.Add(_variableIdentifiers[i], _variables[i]);
        }
    }
}

