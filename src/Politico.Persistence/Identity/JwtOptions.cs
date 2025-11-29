namespace Politico.Persistence.Identity
{
    public sealed class JwtOptions
    {
        /// <summary>Кто выпускает токен (Issuer).</summary>
        public string Issuer { get; set; } = null!;

        /// <summary>Для кого токен (Audience).</summary>
        public string Audience { get; set; } = null!;

        /// <summary>Секретный ключ для подписи (длинная случайная строка).</summary>
        public string SecretKey { get; set; } = null!;

        /// <summary>Сколько минут живёт access-token.</summary>
        public int AccessTokenMinutes { get; set; } = 30;

        /// <summary>Сколько дней живёт refresh-token.</summary>
        public int RefreshTokenDays { get; set; } = 7;
    }
}
