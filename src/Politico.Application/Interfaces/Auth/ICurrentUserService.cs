namespace Politico.Application.Interfaces.Auth
{
    /// <summary>
    /// Информация о текущем аутентифицированном пользователе.
    /// Реализация будет в API слое, читая HttpContext.User.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>Id текущего пользователя или null, если не авторизован.</summary>
        long? UserId { get; }

        /// <summary>Email текущего пользователя (если есть в клеймах).</summary>
        string? Email { get; }

        /// <summary>Роли текущего пользователя.</summary>
        IReadOnlyCollection<string> Roles { get; }

        /// <summary>IP-адрес клиента (для логирования/refresh-tokens).</summary>
        string? IpAddress { get; }
    }
}
