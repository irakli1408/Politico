using Microsoft.Extensions.DependencyInjection;
using Politico.Common.CurrentState;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddScoped<ICurrentStateService, CurrentStateService>();
        return services;
    }
}