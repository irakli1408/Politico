using Politico.Application.Interfaces.Logger;
using Politico.Domain.Entities.ErrorLoger;

namespace Politico.Persistence.Services.Logging
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly AppDbContext _db;

        public ErrorLogService(AppDbContext db) { _db = db; }

        public async Task LogAsync(
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
            CancellationToken ct = default)
        {
            var log = new ErrorLog
            {
                CreatedAtUtc = DateTime.UtcNow,
                Level = level,
                Message = message,
                Exception = exception,
                StackTrace = stackTrace,
                Path = path,
                Method = method,
                StatusCode = statusCode,
                UserId = userId,
                UserIp = userIp,
                UserAgent = userAgent,
                AcceptedLanguage = acceptedLanguage
            };

            _db.ErrorLogs.Add(log);
            await _db.SaveChangesAsync(ct);
        }
    }
}
