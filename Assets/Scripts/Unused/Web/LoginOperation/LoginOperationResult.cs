using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum LoginOperationStatus
{
    Success,
    InvalidUsernameOrPassword,
    ConnectionError,
}

/// <summary>
/// LoginOperationResult contains the results of a LoginOperation.
/// </summary>
public sealed class LoginOperationResult
{
    public int Identifier
    {
        get;
        private set;
    }

    public string AuthenticationToken
    {
        get;
        private set;
    }

    public DateTime? DateDeleted
    {
        get;
        private set;
    }

    public int GameUserIdentifier
    {
        get;
        private set;
    }

    public DateTime DateCreated
    {
        get;
        private set;
    }

    public DateTime DateUpdated
    {
        get;
        private set;
    }

    public LoginOperationStatus LoginOperationStatus
    {
        get;
        private set;
    }

    private LoginOperationResult()
    {
    }

    public sealed class Builder
    {
        private bool _isValid;
        private LoginOperationResult _result;

        private Builder()
        {
            _isValid = true;
            _result = new LoginOperationResult();
        }

        public static Builder Create()
        {
            return new Builder();
        }

        public LoginOperationResult Build()
        {
            _isValid = false;
            VerifyState();
            return _result;
        }

        public Builder Identifier(int identifier)
        {
            AssertPreconditions();
            _result.Identifier = identifier;
            return this;
        }

        public Builder AuthenticationToken(string authenticationToken)
        {
            AssertPreconditions();
            _result.AuthenticationToken = authenticationToken;
            return this;
        }

        public Builder GameUserIdentifier(int gameUserIdentifier)
        {
            AssertPreconditions();
            _result.GameUserIdentifier = gameUserIdentifier;
            return this;
        }

        public Builder DateDeleted(DateTime? dateDeleted)
        {
            AssertPreconditions();
            _result.DateDeleted = dateDeleted;
            return this;
        }

        public Builder DateCreated(DateTime dateCreated)
        {
            AssertPreconditions();
            _result.DateCreated = dateCreated;
            return this;
        }

        public Builder DateUpdated(DateTime dateUpdated)
        {
            AssertPreconditions();
            _result.DateUpdated = dateUpdated;
            return this;
        }

        public Builder LoginOperationStatus(LoginOperationStatus loginOperationStatus)
        {
            AssertPreconditions();
            _result.LoginOperationStatus = loginOperationStatus;
            return this;
        }

        private void AssertPreconditions()
        {
            Asserter.IsTrue(_isValid);
        }

        private void VerifyState()
        {
        }
    }
}