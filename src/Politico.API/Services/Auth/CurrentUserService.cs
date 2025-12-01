using Politico.Application.Interfaces.Auth;
using System.Security.Claims;

namespace Politico.API.Services.Auth
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext? HttpContext => _httpContextAccessor.HttpContext;

        public long? UserId
        {
            get
            {
                var id = HttpContext?.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return long.TryParse(id, out var val) ? val : null;
            }
        }

        public string? Email =>
            HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public IReadOnlyCollection<string> Roles =>
            HttpContext?.User?
                .FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray()
            ?? Array.Empty<string>();

        public string? IpAddress =>
            HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}
