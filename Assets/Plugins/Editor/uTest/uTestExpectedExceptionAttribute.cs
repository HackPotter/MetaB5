using System;

namespace uTest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class uTestExpectedExceptionAttribute : Attribute
    {
        public Type ExceptionType
        {
            get;
            private set;
        }

        public uTestExpectedExceptionAttribute(Type exceptionType)
        {
            ExceptionType = exceptionType;
        }
    }
}