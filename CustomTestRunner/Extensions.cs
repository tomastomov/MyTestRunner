using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTestRunner
{
    public static class Extensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> elements, Action<T> action)
        {
            foreach (var element in elements)
            {
                action(element);
            }

            return elements;
        }
    }
}
