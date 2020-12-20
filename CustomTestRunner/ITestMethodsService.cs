using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CustomTestRunner
{
    public interface ITestMethodsService
    {
        T GetMethodByAttribute<T>(Type testClassType, Type attributeType, Func<MethodInfo[], Type, T> filter);

        string GetMethodId(MethodInfo method);

        IEnumerable<MethodInfo> GetTestMethods(Assembly assembly);

        IEnumerable<MethodInfo> GetTestMethods(Type testClassType);

        MethodInfo GetCleanupMethod(Type testClassType);

        MethodInfo GetSetupMethod(Type testClassType);
    }
}
