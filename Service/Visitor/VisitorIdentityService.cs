using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Service.Visitor
{
    public class VisitorIdentityService : IVisitorIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _salt;

        public VisitorIdentityService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;

            _salt = configuration["Visitor:Salt"]
                ?? throw new InvalidOperationException(
                    "Visitor:Salt tanımlı değil. 'dotnet user-secrets set \"Visitor:Salt\" \"<rastgele-değer>\"' ile ekleyin.");
        }

        public string GetVisitorHash()
        {
            var context = _httpContextAccessor.HttpContext;

            var ip = context?.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip";
            var userAgent = context?.Request.Headers.UserAgent.ToString() ?? "unknown-ua";

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"{ip}|{userAgent}|{_salt}"));
            return Convert.ToHexString(bytes);
        }
    }
}
