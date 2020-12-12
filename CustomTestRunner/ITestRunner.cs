using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTestRunner
{
    public interface ITestRunner
    {
        IEnumerable<TestResult> Run();
    }
}
