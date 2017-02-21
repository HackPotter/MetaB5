using System.Collections.Generic;

public class ExecutionContext
{
    private GlobalSymbolTable _globalSymbolTable;
    private Dictionary<string, object> _localSymbolTable = new Dictionary<string,object>();

    public ExecutionContext(GlobalSymbolTable globalSymbolTable)
    {
        _globalSymbolTable = globalSymbolTable;
    }

    public ExecutionContext(ExecutionContext other)
    {
        _globalSymbolTable = other._globalSymbolTable;

        foreach (var localSymbol in other._localSymbolTable)
        {
            _localSymbolTable.Add(localSymbol.Key, localSymbol.Value);
        }
    }

    public void AddLocal(string identifier, object value)
    {
        _localSymbolTable.Add(identifier, value);
    }

    public T Evaluate<T>(Expression expression)
    {
        if (expression is LiteralExpression)
        {
            object value = (expression as LiteralExpression).Value;
            if (value is T)
            {
                return (T)value;
            }
        }
        else if (expression is VariableExpression)
        {
            string identifier = (expression as VariableExpression).VariableIdentifier;

            if (_localSymbolTable.ContainsKey(identifier))
            {
                object value = _localSymbolTable[identifier];
                if (value is T)
                {
                    return (T)value;
                }
            }
            else
            {
                //Variable variable = _globalSymbolTable.Variables.Find((var) => var.Identifier == identifier);
                Variable variable = _globalSymbolTable.GetVariable(identifier);
                object value = variable.Value;
                if (value is T)
                {
                    return (T)value;
                }
            }
        }
        else
        {
            DebugFormatter.LogError(expression, "Could not evaluate value of expression {0}", expression);
        }

        return default(T);
    }
}

