using System;
using System.Collections.Specialized;
using System.Text;
using UnityEngine;

public enum ResultSetStatus
{
    Success,
    ConnectionError,
    InvalidFormError
}

public class ResultSet
{
    private string _tableName;
    private OrderedDictionary _resultSetData = new OrderedDictionary();
    private ResultSetStatus _resultSetStatus;
    private string _connectionErrorMessage;
    private ResultSetErrorDetails _invalidFormErrorDetails;

    public ResultSetStatus ResultSetStatus
    {
        get { return _resultSetStatus; }
    }

    public ResultSetErrorDetails ErrorDetails
    {
        get { return _invalidFormErrorDetails; }
    }

    public string ConnectionErrorMessage
    {
        get { return _connectionErrorMessage; }
    }

    private ResultSet()
    {
    }

    public string TableName
    {
        get { return _tableName; }
    }

    public DateTime? GetDateTime(int index)
    {
        // todo assert
        return (DateTime?)_resultSetData[index];
    }

    public DateTime? GetDateTime(string key)
    {
        return (DateTime?)_resultSetData[key];
    }

    public int? GetInteger(int index)
    {
        return (int)_resultSetData[index];
    }

    public int? GetInteger(string key)
    {
        return (int)_resultSetData[key];
    }

    public string GetString(int index)
    {
        return (string)_resultSetData[index].ToString();
    }

    public string GetString(string key)
    {
        return (string)_resultSetData[key].ToString();
    }

    private void AddField(string key, object field)
    {
        _resultSetData.Add(key, field);
    }

    public int ResultSetCount
    {
        get { return _resultSetData.Count; }
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var key in _resultSetData)
        {
            stringBuilder.AppendLine("Entry:");
            stringBuilder.AppendLine("Key: " + key.ToString());
            stringBuilder.AppendLine("Value Type: " + _resultSetData[key].GetType());
            stringBuilder.AppendLine("Value: " + _resultSetData[key].ToString());
        }
        return stringBuilder.ToString();
    }

    public class Builder
    {
        private ResultSet _resultSet;

        public static Builder Create()
        {
            return new Builder();
        }

        private Builder()
        {
            _resultSet = new ResultSet();
        }

        public Builder TableName(string tableName)
        {
            _resultSet._tableName = tableName;
            return this;
        }

        public Builder Field(string key, object value)
        {
            _resultSet.AddField(key, value);
            return this;
        }

        public Builder Status(ResultSetStatus resultSetStatus)
        {
            _resultSet._resultSetStatus = resultSetStatus;
            return this;
        }

        public Builder ConnectionErrorMessage(string errorMessage)
        {
            _resultSet._connectionErrorMessage = errorMessage;
            return this;
        }

        public Builder InvalidFormErrorDetails(ResultSetErrorDetails resultSetErrorDetails)
        {
            _resultSet._invalidFormErrorDetails = resultSetErrorDetails;
            return this;
        }

        public ResultSet Build()
        {
            return _resultSet;
        }
    }
}


