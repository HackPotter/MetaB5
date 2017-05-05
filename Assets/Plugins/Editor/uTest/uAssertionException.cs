using System;

namespace uTest
{
    public class uAssertionException : Exception
    {
        public uAssertionException()
        {
        }

        public uAssertionException(string message)
            : base(message)
        {
        }
    }
}