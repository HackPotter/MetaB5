using UnityEngine;
using System.Collections;

public class GetGroupsOperationResult
{
    private ResultSet _resultSet;

    public GetGroupsOperationResult(ResultSet resultSet)
    {
        _resultSet = resultSet;
    }

    public void PrintResultSetData()
    {
        Debug.Log(_resultSet.ToString());
    }
}
