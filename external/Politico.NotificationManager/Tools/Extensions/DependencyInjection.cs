using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Politico.Application.Interfaces.Notificaion;
using Politico.NotificationManager.Email;

namespace Politico.NotificationManager.Tools.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotificationManager(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            // биндим SmtpOptions
            services
                .AddOptions<SmtpOptions>()
                .Bind(configuration.GetSection("Smtp"));

            // выбираем реализацию
            if (env.IsDevelopment())
            {
                services.AddTransient<IEmailSender, SmtpEmailSender>();
            }
            else
            {
                services.AddTransient<IEmailSender, SmtpEmailSender>();
            }

            return services;
        }
    }
}
