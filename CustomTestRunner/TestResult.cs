using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CustomTestRunner
{
    public class TestResult
    {
        public string ClassName { get; set; }

        public MethodInfo Method { get; set; }

        public bool Success { get; set; }

        public List<string> Errors { get; set; }


        public override string ToString()
        {
            return $"{ClassName}, {Method.Name}, {(Success ? "passed" : "failed")}";
        }
    }
}
