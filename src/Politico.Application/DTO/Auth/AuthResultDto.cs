namespace Politico.Application.DTO.Auth
{
    public sealed class AuthResultDto
    {
        public string AccessToken { get; init; } = null!;
        public string RefreshToken { get; init; } = null!;
        public DateTime AccessTokenExpiresAtUtc { get; init; }

        /// <summary>Доп. информация о пользователе, чтобы фронту не делать ещё один запрос.</summary>
        public long UserId { get; init; }
        public string Email { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string[] Roles { get; init; } = Array.Empty<string>();
    }
}
