using CustomTestRunner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestApp;
using WebTestRunner.Hubs;
using WebTestRunner.Models;

namespace WebTestRunner.Controllers
{
    public class TestsController : Controller
    {
        private readonly ITestRunner testsRunner;
        private readonly ITestMethodsService methodsService;
        private readonly IHubContext<TestsHub> hub;

        public TestsController(ITestRunner testsRunner, ITestMethodsService methodsService, IHubContext<TestsHub> hub)
        {
            this.hub = hub;
            this.testsRunner = testsRunner;
            this.methodsService = methodsService;
        }

        public IActionResult Results()
        {
            return View();
        }

        [HttpGet]
        [Route(nameof(RunTests))]
        public IActionResult RunTests()
        {
            var assembly = Assembly.GetAssembly(typeof(Calculator));

            var testsNames = methodsService.GetTestMethods(assembly);

            var viewResults = testsNames.Select(r => new TestResultsViewModel
            {
                TestId = methodsService.GetMethodId(r),
                ClassName = nameof(Calculator),
                MethodName = r.Name,
            });

            Task.Delay(2000).ContinueWith(r => this.testsRunner.Run(assembly, result =>
            {
                hub.Clients.All.SendAsync("SendMessage", result.Id, result.Success);
            }));

            return View(nameof(Results), viewResults);
        }
    }
}
