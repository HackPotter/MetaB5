using System;

namespace uTest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class uTestFixtureTeardownAttribute : Attribute
    {
    }
}