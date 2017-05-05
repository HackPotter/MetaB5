using System;

namespace uTest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class uIgnorePostConditionsAttribute : Attribute
    {
    }
}