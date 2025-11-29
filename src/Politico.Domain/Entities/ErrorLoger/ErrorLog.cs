namespace Politico.Domain.Entities.ErrorLoger
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string Level { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? Exception { get; set; }
        public string? StackTrace { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        public int? StatusCode { get; set; }
        public string? UserId { get; set; }
        public string? UserIp { get; set; }
        public string? UserAgent { get; set; }
        public string? AcceptedLanguage { get; set; }
    }
}
