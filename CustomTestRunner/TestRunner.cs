using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomTestRunner
{
    public class TestRunner : ITestRunner
    {
        private readonly IDictionary<Type, MethodInfo[]> methodsForType;
        public TestRunner()
        {
            methodsForType = new Dictionary<Type, MethodInfo[]>();
        }
        public async Task<IDictionary<string, IEnumerable<TestResult>>> Run(Assembly assembly, Action<TestResult> progressReport = null)
        {
            var classes = assembly
                .GetTypes()
                .Where(t => t.GetCustomAttribute<TestClassAttribute>() != null);

            var testResults = new Dictionary<string, IEnumerable<TestResult>>();

            foreach (var testClass in classes)
            {
                var testMethods = GetTestMethods(testClass);
                var className = testClass.Name;
                testResults[className] = await RunTestMethods(testMethods, testClass, progressReport).ConfigureAwait(false);
            }

            return testResults;
        }

        private async Task<IEnumerable<TestResult>> RunTestMethods(IEnumerable<MethodInfo> testMethods, Type testClass, Action<TestResult> progressReport = null)
        {
            var testResults = new List<TestResult>();
            var classInstance = Activator.CreateInstance(testClass);

            var setupMethod = GetSetupMethod(testClass);

            setupMethod?.Invoke(classInstance, new object[0]);

            foreach (var testMethod in testMethods)
            {
                try
                {
                    var result = testMethod.Invoke(classInstance, new object[0]);

                    if (result is Task t)
                    {
                        await t.ConfigureAwait(false);
                    }

                    testResults.Add(new TestResult() { ClassName = testClass.Name, Method = testMethod, Success = true });
                }
                catch (Exception ex)
                {
                    testResults.Add(new TestResult() { ClassName = testClass.Name, Method = testMethod, Errors = new List<string>() { ex.InnerException.Message } });
                }

                progressReport?.Invoke(testResults.Last());
            }

            var cleanupMethod = GetCleanupMethod(testClass);

            cleanupMethod?.Invoke(classInstance, new object[0]);

            return testResults;
        }

        private MethodInfo GetSetupMethod(Type testClassType)
            => GetMethodByAttribute(testClassType, 
                typeof(TestSetupAttribute), 
                (methods, attributeType) => methods.FirstOrDefault(method => method.GetCustomAttribute(attributeType) != null));

        private MethodInfo GetCleanupMethod(Type testClassType)
            => GetMethodByAttribute(testClassType,
                typeof(TestCleanupAttribute),
                (methods, attributeType) => methods.FirstOrDefault(method => method.GetCustomAttribute(attributeType) != null));
        private IEnumerable<MethodInfo> GetTestMethods(Type testClassType)
            => GetMethodByAttribute(testClassType,
                typeof(TestMethodAttribute),
                (methods, attributeType) => methods.Where(method => method.GetCustomAttribute(attributeType) != null));
        private T GetMethodByAttribute<T>(Type testClassType, Type attributeType, Func<MethodInfo[], Type, T> filter)
        {
            if (!methodsForType.TryGetValue(testClassType, out var methods))
            {
                methods = testClassType.GetMethods();
                methodsForType.Add(testClassType, methods);
            }

            return filter(methods, attributeType);
        }
    }
}
