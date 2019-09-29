using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace VsLiveSanDiego
{
    public class HelloHub : Hub
    {
        public HelloHub(ILogger<HelloHub> logger)
        {
            Logger = logger;
        }

        public ILogger<HelloHub> Logger { get; }
    }
}
