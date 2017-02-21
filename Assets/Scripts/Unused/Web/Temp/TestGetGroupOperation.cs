using UnityEngine;
using System.Collections;

public class TestGetGroupOperation : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        LoginOperationResult result = LoginOperation.Create(LoginCriteria.Create("greg", "asdf")).Execute();

        switch (result.LoginOperationStatus)
        {
            case(LoginOperationStatus.Success):
                break;
            default:
                Debug.LogError("Could not log in!");
                return;
        }

        GetGroupsOperationResult getGroupsResult = GetGroupsOperation.Create(new GetGroupsOperationCriteria(result.AuthenticationToken)).Execute();
        getGroupsResult.PrintResultSetData();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
