﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebTestRunner.Models
{
    public class TestResultsViewModel
    {
        public IEnumerable<TestResultViewModel> TestResults { get; set; }
    }
}
