using System;
using Microsoft.AspNetCore.SignalR;

namespace VsLiveSanDiego.Services
{
    /// <summary>
    /// This class implements the IUserIdProvider interface by simply
    /// returning the user's name. Learn more about SignalR authentication
    /// and authorization via the reference links.
    /// </summary>
    /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.0"/>
    /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.signalr.iuseridprovider"/>
    public class UsernameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
