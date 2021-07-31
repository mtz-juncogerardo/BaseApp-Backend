using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;

namespace BaseApp.Core.Services.CommonServices
{
    public static class IpService
    {
        public static string GetIpAddress(HttpRequest request)
        {
            var ipAddress = request.HttpContext.Connection.RemoteIpAddress;
            if (ipAddress == null)
            {
                return string.Empty;
            }
            if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ipAddress = Dns.GetHostEntry(ipAddress).AddressList
                    .First(r => r.AddressFamily == AddressFamily.InterNetwork);
            }
            return ipAddress.ToString();
        }
    }
}