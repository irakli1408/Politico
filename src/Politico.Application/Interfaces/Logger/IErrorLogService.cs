namespace Politico.Application.Interfaces.Logger
{
    public interface IErrorLogService
    {
        Task LogAsync(
            string level,
            string message,
            string? exception,
            string? stackTrace,
            string? path,
            string? method,
            int? statusCode,
            string? userId,
            string? userIp,
            string? userAgent,
            string? acceptedLanguage,
            CancellationToken ct = default);
    }
}
