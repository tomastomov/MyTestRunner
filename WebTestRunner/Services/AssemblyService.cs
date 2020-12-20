using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebTestRunner.Services
{
    public class AssemblyService : IAssemblyService
    {
        private Assembly assembly;
        public Assembly GetLoadedAssembly()
        {
            return assembly;
        }

        public void SetAssembly(Assembly assembly)
        {
            this.assembly = assembly;
        }
    }
}
