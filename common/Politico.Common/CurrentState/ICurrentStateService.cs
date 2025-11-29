namespace Politico.Common.CurrentState;

public interface ICurrentStateService
{
    string AcceptedLanguage { get; }
    string? UserId { get; }
    string UserIp { get; }
    string Referrer { get; }
    string UserAgent { get; }
}
