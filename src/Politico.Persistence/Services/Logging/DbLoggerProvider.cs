using Microsoft.Extensions.Logging;

namespace Politico.Infrastructure.Logging;

public class DbLoggerProvider : ILoggerProvider
{
    private readonly IServiceProvider _serviceProvider;

    public DbLoggerProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ILogger CreateLogger(string categoryName)
        => new DbLogger(categoryName, _serviceProvider);

    public void Dispose()
    {
    }
}
