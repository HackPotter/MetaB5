using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// RegisterOperation provides the functionality required for the user to register a new account with our server.
/// </summary>
public sealed class RegisterOperation
{
    /// <summary>
    /// Creates a RegisterOperation object.
    /// </summary>
    /// <param name="criteria">Criteria containing the data required to register a new account.</param>
    /// <returns>A RegisterOperation object ready to execute, or null if the criteria contains invalid data.</returns>
    public static RegisterOperation Create(RegisterOperationCriteria criteria)
    {
        Asserter.NotNull(criteria, "RegisterOperation.Create:criteria is null");
        return new RegisterOperation(criteria);
    }

    /// <summary>
    /// Executes the RegisterOperation.
    /// </summary>
    /// <returns>A RegisterOperationResult containing the result of the operation.</returns>
    public RegisterOperationResult Execute()
    {
        ResultSet resultSet = WebOperation.Execute(new WebOperationCriteria(WebOperationURLs.GetURL(GetType()), CreateFieldDictionaryFromCriteria(_criteria)));

        return CreateResultFromResultSet(resultSet);
    }

    /// <summary>
    /// Executes the RegisterOperation asynchronously. The RegisterOperationCallback will be invoked when the operation has completed.
    /// </summary>
    /// <param name="registerOperationCallback">The RegisterOperationCallback to be invoked when the operation completes.</param>
    public void ExecuteAsync(RegisterOperationCallback registerOperationCallback)
    {
        Asserter.NotNull(registerOperationCallback, "RegisterOperation.ExecuteAsync:registerOperationCallback is null");
        WebOperationCallback webOperationCallback = (ResultSet results) =>
        {
            RegisterOperationResult result = CreateResultFromResultSet(results);
            registerOperationCallback(result);
        };

        WebOperation.ExecuteAsync(new WebOperationCriteria(WebOperationURLs.GetURL(GetType()), CreateFieldDictionaryFromCriteria(_criteria)), webOperationCallback);
    }

    private RegisterOperationCriteria _criteria;

    /// <summary>
    /// Constructs a new RegisterOperation.
    /// </summary>
    /// <param name="criteria">The RegisterOperationCriteria containing the data required to complete the operation.</param>
    private RegisterOperation(RegisterOperationCriteria criteria)
    {
        _criteria = criteria;
    }

    /// <summary>
    /// Creates a RegisterOperationResult from the WWW returned by the WebOperation.
    /// </summary>
    /// <param name="www">The WWW containing the result of the web operation.</param>
    /// <returns>A RegisterOperationResult containing the result of the operation.</returns>
    private static RegisterOperationResult CreateResultFromResultSet(ResultSet resultSet)
    {
        return new RegisterOperationResult(resultSet);
    }

    /// <summary>
    /// Creates a Dictionary of text field keys and values to insert into the WWWForm before executing the web operation.
    /// </summary>
    /// <param name="criteria">The RegisterOperationCriteria from which to generate the WWWForm text fields.</param>
    /// <returns>A Dictionary mapping text keys to text values from the RegisterOperationCriteria.</returns>
    private static Dictionary<string, string> CreateFieldDictionaryFromCriteria(RegisterOperationCriteria criteria)
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();

        fields.Add("username", criteria.Username);
        fields.Add("age", criteria.DateOfBirth.ToShortDateString());
        fields.Add("email", criteria.Email);
        fields.Add("password", criteria.Password);
        fields.Add("password_confirmation", criteria.Password);
        fields.Add("gender", GenderEnumToString(criteria.Gender));
        fields.Add("postal_code", criteria.ZipCode);

        return fields;
    }

    /// <summary>
    /// Converts a RegistrationCriteriaGender enum value to a string value interprettable by the server.
    /// </summary>
    /// <param name="gender">The RegistrationCriteriaGender enum to convert to a string.</param>
    /// <returns>"M" if the RegistrationCriteriaGender indicates male, "F" if the RegistrationCriteriaGender indicates female, or "?" if the RegistrationCriteriaGender indicates neither male nor female.</returns>
    private static string GenderEnumToString(RegistrationCriteriaGender gender)
    {
        switch (gender)
        {
            case(RegistrationCriteriaGender.Male):
                return "M";
            case(RegistrationCriteriaGender.Female):
                return "F";
            default:
                return "?";
        }
    }
}

