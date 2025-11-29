using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Politico.Application.Interfaces.Logger;

namespace Politico.Infrastructure.Logging;

public class DbLogger : ILogger
{
    private readonly string _categoryName;
    private readonly IServiceProvider _serviceProvider;

    public DbLogger(string categoryName, IServiceProvider serviceProvider)
    {
        _categoryName = categoryName;
        _serviceProvider = serviceProvider;
    }

    public IDisposable? BeginScope<TState>(TState state) => null;

    // Что именно пишем в базу — Warning и выше
    public bool IsEnabled(LogLevel logLevel) =>
        logLevel >= LogLevel.Warning;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);

        // Создаём scope, чтобы взять scoped-DbContext/сервис
        using var scope = _serviceProvider.CreateScope();
        var errorLogService = scope.ServiceProvider.GetRequiredService<IErrorLogService>();

        try
        {
            errorLogService.LogAsync(
                    level: logLevel.ToString(),
                    message: message,
                    exception: exception?.Message,
                    stackTrace: exception?.StackTrace,
                    path: null,          
                    method: null,
                    statusCode: null,
                    userId: null,
                    userIp: null,
                    userAgent: null,
                    acceptedLanguage: null
                )
                .GetAwaiter()
                .GetResult(); // ILogger синхронный, поэтому так
        }
        catch
        {
            // Тут НИЧЕГО не выбрасываем, чтобы логгер не уронил приложение
        }
    }
}


