using System;

namespace uTest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class uTestAttribute : Attribute
    {
        public int Priority
        {
            get;
            private set;
        }
    }
}