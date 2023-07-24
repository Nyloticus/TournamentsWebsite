using Microsoft.AspNetCore.SignalR;
using System;

namespace Infrastructure
{
    public class CustomUserProvider : IUserIdProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomUserProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}