using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTestRunner
{
    public static class Assert
    {
        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                throw new ArgumentException("Assertion failed!");
            }
        }

        public static void IsTrue(Func<bool> condition)
        {
            if (!condition())
            {
                throw new ArgumentException("Assertion failed!");
            }
        }

        public static void AreEqual(object a, object b)
        {
            if (a != b)
            {
                throw new ArgumentException("Assertion failed");
            }
        }

        public static void AreEqual<T>(T a, T b) where T : IComparable
        {
            if (a.CompareTo(b) != 0)
            {
                throw new ArgumentException("Assertion failed!");
            }
        }

        public static void ThrowsException<T>(Action action) where T : Exception
        {
            var hasThrownExcepion = false;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(T))
                {
                    throw new ArgumentException("Assertion failed!");
                }
                hasThrownExcepion = true;
            }

            if (!hasThrownExcepion)
            {
                throw new ArgumentException("Assertion failed!");
            }
        }
    }
}
