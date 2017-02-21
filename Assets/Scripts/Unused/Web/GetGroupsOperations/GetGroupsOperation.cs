using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetGroupsOperation
{
    private GetGroupsOperationCriteria _getGroupsOperationCriteria;

    private GetGroupsOperation(GetGroupsOperationCriteria getGroupsOperationCriteria)
    {
        _getGroupsOperationCriteria = getGroupsOperationCriteria;
    }

    public static GetGroupsOperation Create(GetGroupsOperationCriteria getGroupsOperationCriteria)
    {
        Asserter.NotNull(getGroupsOperationCriteria, "GetGroupOperation.Create:GetGroupsOperationCriteria is null");
        return new GetGroupsOperation(getGroupsOperationCriteria);
    }

    public GetGroupsOperationResult Execute()
    {
        return new GetGroupsOperationResult(WebOperation.Execute(new WebOperationCriteria(WebOperationURLs.GetURL(GetType()),GetFieldDictionaryFromCriteria(_getGroupsOperationCriteria))));
    }

    private static Dictionary<string, string> GetFieldDictionaryFromCriteria(GetGroupsOperationCriteria criteria)
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();

        fields.Add("token", criteria.AuthenticationToken);

        return fields;
    }
}
