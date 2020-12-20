using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomTestRunner
{
    public class TestMethodsService : ITestMethodsService
    {
        private readonly ConcurrentDictionary<Type, MethodInfo[]> methodsForType;
        private readonly ConcurrentDictionary<MethodInfo, string> idsPerMethod;
        public TestMethodsService()
        {
            methodsForType = new ConcurrentDictionary<Type, MethodInfo[]>();
            idsPerMethod = new ConcurrentDictionary<MethodInfo, string>();
        }

        public T GetMethodByAttribute<T>(Type testClassType, Type attributeType, Func<MethodInfo[], Type, T> filter)
        {
            var methods = methodsForType.GetOrAdd(testClassType, key => key.GetMethods());

            return filter(methods, attributeType);
        }
        public MethodInfo GetCleanupMethod(Type testClassType)
           => GetMethodByAttribute(testClassType,
               typeof(TestCleanupAttribute),
               (methods, attributeType) => methods.FirstOrDefault(method => method.GetCustomAttribute(attributeType) != null));

        public MethodInfo GetSetupMethod(Type testClassType)
            => GetMethodByAttribute(testClassType,
                typeof(TestSetupAttribute),
                (methods, attributeType) => methods.FirstOrDefault(method => method.GetCustomAttribute(attributeType) != null));

        public IEnumerable<MethodInfo> GetTestMethods(Type testClassType)
            => GetMethodByAttribute(testClassType,
                typeof(TestMethodAttribute),
                (methods, attributeType) => methods.Where(method => method.GetCustomAttribute(attributeType) != null));

        public IEnumerable<MethodInfo> GetTestMethods(Assembly assembly)
        {
            var allTestMethods = new List<MethodInfo>();

            assembly.GetTypes().Where(t => t.GetCustomAttribute<TestClassAttribute>() != null).Each(type => allTestMethods.AddRange(GetTestMethods(type)));

            return allTestMethods; 
        }

        public string GetMethodId(MethodInfo method)
        {
            var id = idsPerMethod.GetOrAdd(method, (key) => Guid.NewGuid().ToString());

            return id;
        }
    }
}
