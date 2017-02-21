using System.Collections.Generic;
using System;

namespace uTest
{
    public static class Assert
    {
        public delegate bool TrueForAllDelegate<T>(T element);
        public delegate string AssertionFailedMessageDelegate<T>(T element);

        public static int AssertionCount
        {
            get;
            private set;
        }

        public static void ResetAssertionCount()
        {
            AssertionCount = 0;
        }

        public static void RefEqual(object o1, object o2, string message = "", params object[] args)
        {
            AssertionCount++;
            if (o1 == o2)
                return;

            if (string.IsNullOrEmpty(message))
            {
                message = string.Format("Expected {0} and {1} to be reference equal.", o1, o2);
            }
            else
            {
                message = string.Format(message, args);
            }
            throw new uAssertionException(message);
        }

        public static void True(bool condition, string message = "", params object[] args)
        {
            AssertionCount++;
            if (condition)
                return;

            throw new uAssertionException(string.Format(message, args));
        }

        public static void False(bool condition, string message = "", params object[] args)
        {
            AssertionCount++;
            if (!condition)
                return;

            throw new uAssertionException(string.Format(message, args));
        }

        public static void TrueForAll<T>(IEnumerable<T> enumerable, TrueForAllDelegate<T> condition, AssertionFailedMessageDelegate<T> message = null)
        {
            AssertionCount++;
            foreach (T element in enumerable)
            {
                if (!condition(element))
                {
                    throw new uAssertionException(message != null ? message(element) : "");
                }
            }
        }

        public static void AreEqual(bool expected, bool actual, string message = "", params object[] args)
        {
            AssertionCount++;
            if (expected == actual)
                return;

            if (string.IsNullOrEmpty(message))
            {
                message = string.Format("Expected {0}, but was {1}.", expected, actual);
            }
            else
            {
                message = string.Format(message, args);
            }
            throw new uAssertionException(message);
        }

        public static void AreEqual(int expected, int actual, string message = "", params object[] args)
        {
            AssertionCount++;
            if (expected == actual)
                return;

            if (string.IsNullOrEmpty(message))
            {
                message = string.Format("Expected {0}, but was {1}.", expected, actual);
            }
            else
            {
                message = string.Format(message, args);
            }
            throw new uAssertionException(message);
        }

        public static void NotRefEqual(object first, object second, string message = "", params object[] args)
        {
            AssertionCount++;
            if (!object.ReferenceEquals(first, second))
                return;

            if (string.IsNullOrEmpty(message))
            {
                message = string.Format("Expected {0} and {1} to be not reference equal.", first, second);
            }
            else
            {
                message = string.Format(message, args);
            }
            throw new uAssertionException(message);
        }

        public static void AreEqual(string expected, string actual, string message = "", params object[] args)
        {
            AssertionCount++;
            if (expected == actual)
                return;

            if (string.IsNullOrEmpty(message))
            {
                message = string.Format("Expected {0}, but was {1}.", expected, actual);
            }
            else
            {
                message = string.Format(message, args);
            }
            throw new uAssertionException(message);
        }

        public static void AreEqual(Type expected, Type actual, string message = "", params object[] args)
        {
            AssertionCount++;
            if (expected == actual)
            {
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                message = string.Format("Expected {0}, but was {1}.", expected, actual);
            }
            else
            {
                message = string.Format(message, args);
            }
            throw new uAssertionException(message);
        }

        public static void NotNull(object value, string message = "", params object[] args)
        {
            AssertionCount++;
            if (value != null || (value is UnityEngine.Object && (value as UnityEngine.Object)))
                return;

            throw new uAssertionException(string.Format(message, args));
        }
    }
}