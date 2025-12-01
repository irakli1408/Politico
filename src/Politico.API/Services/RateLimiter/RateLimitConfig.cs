using Politico.Common.Model;

namespace Politico.API.Services.RateLimiter
{
    public static class RateLimitConfig
    {
        private static readonly Dictionary<string, RateLimitPolicyOptions> _policies = new();

        public static void Initialize(IConfiguration configuration)
        {
            // читаем все политики из секции RateLimiting:Policies
            var policiesSection = configuration.GetSection("RateLimiting:Policies");
            foreach (var child in policiesSection.GetChildren())
            {
                var options = child.Get<RateLimitPolicyOptions>();
                if (options != null)
                {
                    _policies[child.Key] = options;
                }
            }
        }

        public static RateLimitPolicyOptions Get(string name)
        {
            if (!_policies.TryGetValue(name, out var options))
            {
                throw new InvalidOperationException(
                    $"Rate limit policy '{name}' is not configured in appsettings.");
            }

            return options;
        }
    }
}
