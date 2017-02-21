using UnityEngine;
using System.Collections;

public sealed class GetGroupsOperationCriteria
{
    private string _authenticationToken;

    public string AuthenticationToken
    {
        get { return _authenticationToken; }
    }

    public GetGroupsOperationCriteria(string authenticationToken)
    {
        _authenticationToken = authenticationToken;
    }
}