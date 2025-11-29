using Politico.Domain.Entities.Auth;

namespace Politico.Application.Interfaces.Auth
{
    /// <summary>
    /// Сервис генерации токенов (JWT access + refresh).
    /// Реализация будет в Infrastructure/Auth.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Сгенерировать access token (обычно JWT) по пользователю и его ролям.
        /// Возвращает сам токен и время его истечения.
        /// </summary>
        (string AccessToken, DateTime ExpiresAtUtc) GenerateAccessToken(
            User user,
            IReadOnlyCollection<Role> roles);

        /// <summary>
        /// Сгенерировать новый refresh token для пользователя.
        /// </summary>
        RefreshToken GenerateRefreshToken(long userId, string createdByIp);
    }
}

