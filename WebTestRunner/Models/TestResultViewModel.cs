using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTestRunner.Models
{
    public class TestResultViewModel
    {
        public string TestId { get; set; }
        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public bool Success { get; set; }

        public List<string> Errors { get; set; }
    }
}
