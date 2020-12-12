using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomTestRunner
{
    public class TestRunner : ITestRunner
    {
        public IEnumerable<TestResult> Run()
        {
            var classes = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<TestClassAttribute>() != null);

            var testResults = new List<TestResult>().AsEnumerable();

            foreach (var testClass in classes)
            {
                var testMethods = GetTestMethods(testClass);
                testResults = RunTestMethods(testMethods, testClass);
            }

            return testResults;
        }

        private IEnumerable<TestResult> RunTestMethods(IEnumerable<MethodInfo> testMethods, Type testClass)
        {
            var testResults = new List<TestResult>();
            try
            {
                var classInstance = Activator.CreateInstance(testClass);

                foreach (var testMethod in testMethods)
                {
                    var parameters = testMethod.GetParameters();
                    var result = testMethod.Invoke(classInstance, new object[0]);
                    testResults.Add(new TestResult() { ClassName = testClass.Name, Method = testMethod, Success = true });
                }

            }
            catch (Exception ex)
            {
                testResults.Add(new TestResult() { ClassName = testClass.Name, Errors = new List<string>() { ex.Message } });
            }

            return testResults;
        }

        private IEnumerable<MethodInfo> GetTestMethods(Type testClassType)
            => testClassType.GetMethods().Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null);
    }
}
