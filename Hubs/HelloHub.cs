using System;
using System.Threading.Tasks;
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

        public override Task OnConnectedAsync()
        {
            Logger.LogInformation($"User {Context.User.Identity.Name} connected");
            return base.OnConnectedAsync();
        }
    }
}
