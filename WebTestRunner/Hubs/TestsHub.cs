using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTestRunner.Hubs
{
    public class TestsHub : Hub
    {
        public Task SendMessage(string testId, bool isSuccessful)
            => Clients.All.SendAsync("TestFinished", testId, isSuccessful);
    }   
}
