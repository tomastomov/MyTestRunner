using CustomTestRunner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using TestApp;
using WebTestRunner.Hubs;
using WebTestRunner.Models;
using WebTestRunner.Services;

namespace WebTestRunner.Controllers
{
    public class TestsController : Controller
    {
        private readonly ITestRunner testsRunner;
        private readonly ITestMethodsService methodsService;
        private readonly IHubContext<TestsHub> hub;
        private readonly IAssemblyService assemblyService;

        public TestsController(ITestRunner testsRunner, ITestMethodsService methodsService, IHubContext<TestsHub> hub, IAssemblyService assemblyService)
        {
            this.hub = hub;
            this.assemblyService = assemblyService;
            this.testsRunner = testsRunner;
            this.methodsService = methodsService;
        }

        public IActionResult Explorer(FileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var assembly = AssemblyLoadContext.Default.LoadFromStream(model.Dll.OpenReadStream());

            assemblyService.SetAssembly(assembly);

            var testsNames = methodsService.GetTestMethods(assembly);

            var viewResults = testsNames.Select(r => new TestResultViewModel
            {
                TestId = methodsService.GetMethodId(r),
                ClassName = nameof(Calculator),
                MethodName = r.Name,
            });

            return View(viewResults);
        }

        [Route(nameof(RunTests))]
        public async Task<IActionResult> RunTests()
        {
            var results = await this.testsRunner.Run(assemblyService.GetLoadedAssembly(), result =>
            {
                hub.Clients.All.SendAsync("SendMessage", result.Id, result.Success);
            }).ConfigureAwait(false);

            return Ok(results);
        }
    }
}
