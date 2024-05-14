using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;

namespace Discussify.API.Extensions
{
    public static class ServerExtension
    {
        public static string GetHostUrl(this IServer server)
        {
            var addresses = server?.Features.Get<IServerAddressesFeature>();
            var baseUrl = addresses?.Addresses?.FirstOrDefault() ?? string.Empty;
            int lastIndex = baseUrl.LastIndexOf('/');
            return baseUrl.Remove(lastIndex);
        }
    }
}
