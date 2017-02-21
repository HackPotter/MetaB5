using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// RegistrationCriteriaGender enumerates the accepted genders on the registration form.
/// </summary>
public enum RegistrationCriteriaGender
{
    Male,
    Female
}

/// <summary>
/// RegisterOperationCriteria contains data required to execute a RegisterOperation.
/// </summary>
public sealed class RegisterOperationCriteria
{
    /// <summary>
    /// Constructs a new RegisterOperationCriteria.
    /// </summary>
    private RegisterOperationCriteria()
    {
    }

    /// <summary>
    /// Gets the username to use for the RegisterOperation.
    /// </summary>
    public string Username
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the date of birth to use for the RegisterOperation.
    /// </summary>
    public DateTime DateOfBirth
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the e-mail address to use for the RegisterOperation.
    /// </summary>
    public string Email
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the password to use for the RegisterOperation.
    /// </summary>
    public string Password
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the gender to use for the RegisterOperation.
    /// </summary>
    public RegistrationCriteriaGender Gender
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the zip code to use for the RegisterOperation.
    /// </summary>
    public string ZipCode
    {
        get;
        private set;
    }

    /// <summary>
    /// RegisterOperationCriteria.Builder is used to construct a new RegisterOperationCriteria.
    /// 
    /// <br />
    /// <br />
    /// 
    /// The Builder is instantiated with the static method Create(). After instantiation, parameters can be included by making the appropriate method calls.
    /// Each method returns the Builder instance, so that multiple parameters can be included in one statement.
    /// 
    /// <br />
    /// Example:
    /// <br />
    /// <code>Builder.Create().Username("TestUser").Password("Password").Email("emailuser@emailservice.net").Build()</code>
    /// </summary>
    public class Builder
    {
        // The criteria we're actively building.
        private RegisterOperationCriteria _criteria;

        // Used to ensure that a given Builder instance is used only once.
        private bool _isValid;

        /// <summary>
        /// Constructs a RegisterOperationCriteria.Builder.
        /// </summary>
        private Builder()
        {
            _isValid = true;
            _criteria = new RegisterOperationCriteria();
        }

        /// <summary>
        /// Creates a new RegisterOperationCriteria.Builder object.
        /// </summary>
        /// <returns>The Builder being constructed.</returns>
        public static Builder Create()
        {
            return new Builder();
        }

        /// <summary>
        /// Includes the given username with the actively constructed RegisterOperationCriteria.
        /// </summary>
        /// <param name="username">The username to include in the RegisterOperationCriteria.</param>
        /// <returns>The Builder.</returns>
        public Builder Username(string username)
        {
            VerifyPreconditions();
            _criteria.Username = username;
            return this;
        }

        /// <summary>
        /// Includes the given password with the actively constructed RegisterOperationCriteria.
        /// </summary>
        /// <param name="password">The password to include in the RegisterOperationCriteria.</param>
        /// <returns>The Builder.</returns>
        public Builder Password(string password)
        {
            VerifyPreconditions();
            _criteria.Password = password;
            return this;
        }

        /// <summary>
        /// Includes the given date of birth with the actively constructed RegisterOperationCriteria.
        /// </summary>
        /// <param name="date">The date of birth to include in the RegisterOperationCriteria.</param>
        /// <returns>The Builder.</returns>
        public Builder DateOfBirth(DateTime date)
        {
            VerifyPreconditions();
            _criteria.DateOfBirth = date;
            return this;
        }

        /// <summary>
        /// Includes the given e-mail address with the actively constructed RegisterOperationCriteria.
        /// </summary>
        /// <param name="email">The e-mail address to include in the RegisterOperationCriteria.</param>
        /// <returns>The Builder.</returns>
        public Builder Email(string email)
        {
            VerifyPreconditions();
            _criteria.Email = email;
            return this;
        }

        /// <summary>
        /// Includes the given gender with the actively constructed RegisterOperationCriteria.
        /// </summary>
        /// <param name="gender">The RegistrationCriteriaGender enum value indicating the gender to be included in the RegisterOperationCriteria.</param>
        /// <returns>The Builder.</returns>
        public Builder Gender(RegistrationCriteriaGender gender)
        {
            VerifyPreconditions();
            _criteria.Gender = gender;
            return this;
        }

        /// <summary>
        /// Includes the given postal zip code with the actively constructed RegisterOperationCriteria.
        /// </summary>
        /// <param name="zipCode">The zip code to include in the RegisterOperationCriteria.</param>
        /// <returns>The Builder.</returns>
        public Builder ZipCode(string zipCode)
        {
            VerifyPreconditions();
            _criteria.ZipCode = zipCode;
            return this;
        }

        /// <summary>
        /// Finishes building of the RegisterOperationCriteria.
        /// </summary>
        /// <returns>The finished RegisterOperationCriteria object.</returns>
        public RegisterOperationCriteria Build()
        {
            VerifyPreconditions();
            _isValid = false;
            VerifyCriteria(_criteria);
            return _criteria;
        }

        private void VerifyPreconditions()
        {
            Asserter.IsTrue(_isValid);
        }

        /// <summary>
        /// Verifies that the constructed RegisterOperationCriteria is valid.
        /// </summary>
        /// <param name="criteria">The RegisterOperationCriteria to inspect.</param>
        private static void VerifyCriteria(RegisterOperationCriteria criteria)
        {
            Asserter.NotNullOrEmpty(criteria.Username, "RegisterOperationCriteria.Builder:username is null or empty");
            Asserter.NotNullOrEmpty(criteria.Password, "RegisterOperationCriteria.Builder:password is null or empty");
            Asserter.NotNullOrEmpty(criteria.ZipCode, "RegisterOperationCriteria.Builder:zipcode is null or empty");
        }
    }
}

