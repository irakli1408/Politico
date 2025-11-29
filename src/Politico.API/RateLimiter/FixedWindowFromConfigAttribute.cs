using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Politico.API.RateLimiter;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public sealed class FixedWindowFromConfigAttribute : Attribute, IRateLimiterPolicy<string>
{
    public string PolicyName { get; }

    public FixedWindowFromConfigAttribute(string policyName)
    {
        PolicyName = policyName;
    }

    // можем использовать глобальный OnRejected из AddRateLimiter
    Func<OnRejectedContext, CancellationToken, ValueTask>? IRateLimiterPolicy<string>.OnRejected => null;

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        var cfg = RateLimitConfig.Get(PolicyName); // берём из appsettings

        var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: key,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = cfg.PermitLimit,
                Window = TimeSpan.FromSeconds(cfg.WindowSeconds),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            });
    }
}
