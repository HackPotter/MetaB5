using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetablastServer.Codes
{
    public enum RegisterUserParameters : byte
    {
        Email = 0,
        Password = 1,
        Sex = 2,
        Age = 3,
        Education = 4,
        Country = 5,
        AnalyticsEnabled = 6,
    }

    public enum RegisterUserErrorDetail : byte
    {
        DatabaseError = 6,
        InvalidLastName = 5,
        InvalidFirstName = 4,
        InvalidPassword = 3,
        InvalidEmail = 2,
        EmailInUse = 1,
        Success = 0,
    }
}
