using Microsoft.AspNetCore.Http;
using Politico.Common.CurrentState;

public class CurrentStateService : ICurrentStateService
{
    public CurrentStateService(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;

        AcceptedLanguage = context?.Request?.Headers["Accept-Language"].FirstOrDefault() ?? "en";
        UserIp = context?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        UserId = context?.User?.Identity?.Name;
        Referrer = context?.Request?.Headers["Referer"].FirstOrDefault() ?? string.Empty;
        UserAgent = context?.Request?.Headers["User-Agent"].FirstOrDefault() ?? string.Empty;
    }

    public string AcceptedLanguage { get; }
    public string? UserId { get; }
    public string UserIp { get; }
    public string Referrer { get; }
    public string UserAgent { get; }
}