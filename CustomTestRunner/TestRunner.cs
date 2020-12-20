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
        private readonly ITestMethodsService methodsService_;
        public TestRunner(ITestMethodsService methodsService = null)
        {
            methodsService_ = methodsService ?? new TestMethodsService();
        }

        public async Task<IDictionary<string, IEnumerable<TestResult>>> Run(Assembly assembly, Action<TestResult> progressReport = null)
        {
            var classes = assembly
                .GetTypes()
                .Where(t => t.GetCustomAttribute<TestClassAttribute>() != null);

            var testResults = new Dictionary<string, IEnumerable<TestResult>>();

            foreach (var testClass in classes)
            {
                var testMethods = methodsService_.GetTestMethods(testClass);
                var className = testClass.Name;
                testResults[className] = await RunTestMethods(testMethods, testClass, progressReport).ConfigureAwait(false);
            }

            return testResults;
        }

        private async Task<IEnumerable<TestResult>> RunTestMethods(IEnumerable<MethodInfo> testMethods, Type testClass, Action<TestResult> progressReport = null)
        {
            var testResults = new List<TestResult>();
            var classInstance = Activator.CreateInstance(testClass);

            var setupMethod = methodsService_.GetSetupMethod(testClass);

            setupMethod?.Invoke(classInstance, new object[0]);

            foreach (var testMethod in testMethods)
            {
                var id = methodsService_.GetMethodId(testMethod);

                try
                {
                    var result = testMethod.Invoke(classInstance, new object[0]);

                    if (result is Task t)
                    {
                        await t.ConfigureAwait(false);
                    }

                    testResults.Add(new TestResult() { Id = id, ClassName = testClass.Name, Method = testMethod, Success = true });
                }
                catch (Exception ex)
                {
                    testResults.Add(new TestResult() { Id = id, ClassName = testClass.Name, Method = testMethod, Errors = new List<string>() { ex.InnerException.Message } });
                }

                progressReport?.Invoke(testResults.Last());
            }

            var cleanupMethod = methodsService_.GetCleanupMethod(testClass);

            cleanupMethod?.Invoke(classInstance, new object[0]);

            return testResults;
        }
    }
}
