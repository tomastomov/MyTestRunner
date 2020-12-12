using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTestRunner
{
    public interface ITestRunner
    {
        IDictionary<string, IEnumerable<TestResult>> Run();
    }
}
