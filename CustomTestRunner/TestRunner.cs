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

            foreach (var testMethod in testMethods)
            {
                try
                {
                    var parameters = testMethod.GetParameters();
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

            return testResults;
        }

        private IEnumerable<MethodInfo> GetTestMethods(Type testClassType)
            => testClassType.GetMethods().Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null);
    }
}
