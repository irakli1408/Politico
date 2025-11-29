namespace Politico.Common.ErrorHandler.Models;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
}