using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomTestRunner
{
    public class TestRunner : ITestRunner
    {
        public IDictionary<string, IEnumerable<TestResult>> Run()
        {
            var classes = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<TestClassAttribute>() != null);

            var testResults = new Dictionary<string, IEnumerable<TestResult>>();

            foreach (var testClass in classes)
            {
                var testMethods = GetTestMethods(testClass);
                var className = testClass.Name;
                testResults[className] = RunTestMethods(testMethods, testClass);
            }

            return testResults;
        }

        private IEnumerable<TestResult> RunTestMethods(IEnumerable<MethodInfo> testMethods, Type testClass)
        {
            var testResults = new List<TestResult>();
            var classInstance = Activator.CreateInstance(testClass);

            foreach (var testMethod in testMethods)
            {
                try
                {
                    var parameters = testMethod.GetParameters();
                    var result = testMethod.Invoke(classInstance, new object[0]);
                    testResults.Add(new TestResult() { ClassName = testClass.Name, Method = testMethod, Success = true });
                }
                catch (Exception ex)
                {
                    testResults.Add(new TestResult() { ClassName = testClass.Name, Method = testMethod, Errors = new List<string>() { ex.InnerException.Message } });
                }
            }

            return testResults;
        }

        private IEnumerable<MethodInfo> GetTestMethods(Type testClassType)
            => testClassType.GetMethods().Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null);
    }
}
