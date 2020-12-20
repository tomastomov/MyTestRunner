using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebTestRunner.Services
{
    public interface IAssemblyService
    {
        Assembly GetLoadedAssembly();

        void SetAssembly(Assembly assembly);
    }
}
