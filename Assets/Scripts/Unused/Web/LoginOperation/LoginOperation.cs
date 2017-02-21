using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LoginOperation provides the functionality required for the user to login to our server.
/// </summary>
public sealed class LoginOperation
{
    // ResultSet keys
    private const string kID_KEY = "id";
    private const string kTOKEN_KEY = "token";
    private const string kDELETED_AT_KEY = "deleted_at";
    private const string kGAME_USER_ID_KEY = "game-user-id";
    private const string kCREATED_AT_KEY = "created-at";
    private const string kUPDATED_AT_KEY = "updated-at";

    // Fields keys
    private const string kUSERNAME_FIELD = "username";
    private const string kPASSWORD_FIELD = "password";

    /// <summary>
    /// Creates a LoginOperation object.
    /// </summary>
    /// <param name="criteria">Criteria containing the data required to login.</param>
    /// <returns>A LoginOperation object ready to execute.</returns>
    /// <throws>AssertionFailureException if the given criteria is null.</throws>
    public static LoginOperation Create(LoginCriteria criteria)
    {
        Asserter.NotNull(criteria, "LoginOperation.Create:criteria is null");
        return new LoginOperation(criteria);
    }

    /// <summary>
    /// Executes the LoginOperation.
    /// </summary>
    /// <returns>A LoginOperationResult containing the result of the operation.</returns>
    public LoginOperationResult Execute()
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();

        fields.Add(kUSERNAME_FIELD, _criteria.Username);
        fields.Add(kPASSWORD_FIELD, _criteria.Password);

        ResultSet resultSet = WebOperation.Execute(new WebOperationCriteria(WebOperationURLs.GetURL(GetType()), fields));

        return CreateResultFromResultSet(resultSet);
    }

    /// <summary>
    /// Executes the LoginOperation asynchronously. The LoginOperationCallback will be invoked when the results are ready.
    /// </summary>
    /// <param name="loginOperationCallback">The callback to be invoked when the result of the LoginOperation is ready.</param>
    public void ExecuteAsync(LoginOperationCallback loginOperationCallback)
    {
        Asserter.NotNull(loginOperationCallback, "LoginOperation.ExecuteAsync:loginOperationCallback is null");
        Dictionary<string, string> fields = new Dictionary<string, string>();

        fields.Add(kUSERNAME_FIELD, _criteria.Username);
        fields.Add(kPASSWORD_FIELD, _criteria.Password);

        WebOperationCallback webOperationCallback = (ResultSet resultSet) =>
        {
            LoginOperationResult result = CreateResultFromResultSet(resultSet);
            loginOperationCallback(result);
        };

        WebOperation.ExecuteAsync(new WebOperationCriteria(WebOperationURLs.GetURL(GetType()), fields), webOperationCallback);
    }

    private LoginCriteria _criteria;

    /// <summary>
    /// Constructs a new LoginOperation.
    /// </summary>
    /// <param name="criteria">The LoginCriteria containing the data required to complete the operation.</param>
    private LoginOperation(LoginCriteria criteria)
    {
        _criteria = criteria;
    }

    /// <summary>
    /// Creates a LoginOperationResult from the WWW returned by the WebOperation.
    /// </summary>
    /// <param name="www">The WWW containing the results of the web operation.</param>
    /// <returns>A LoginOperationResult containing the results of the LoginOperation.</returns>
    private static LoginOperationResult CreateResultFromResultSet(ResultSet resultSet)
    {
        LoginOperationResult.Builder builder = LoginOperationResult.Builder.Create();

        switch (resultSet.ResultSetStatus)
        {
            case(ResultSetStatus.ConnectionError):
                builder.LoginOperationStatus(LoginOperationStatus.ConnectionError);
                return builder.Build();
            case(ResultSetStatus.InvalidFormError):
                builder.LoginOperationStatus(LoginOperationStatus.InvalidUsernameOrPassword);
                return builder.Build();
            case(ResultSetStatus.Success):
                builder.LoginOperationStatus(LoginOperationStatus.Success);
                break;
        }


        return builder
               .Identifier(resultSet.GetInteger(kID_KEY).Value)
               .AuthenticationToken(resultSet.GetString(kTOKEN_KEY))
               .DateDeleted(resultSet.GetDateTime(kDELETED_AT_KEY))
               .GameUserIdentifier(resultSet.GetInteger(kGAME_USER_ID_KEY).Value)
               .DateCreated(resultSet.GetDateTime(kCREATED_AT_KEY).Value)
               .DateUpdated(resultSet.GetDateTime(kUPDATED_AT_KEY).Value)
               .Build();

    }
}