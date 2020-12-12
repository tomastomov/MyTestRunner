using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomTestRunner
{
    public interface ITestRunner
    {
        Task<IDictionary<string, IEnumerable<TestResult>>> Run(Assembly assembly, Action<TestResult> progressReport = null);
    }
}
