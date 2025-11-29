namespace Politico.Domain.Entities.Auth
{
    public class RefreshToken
    {
        private RefreshToken() { }

        public RefreshToken(string token, DateTime expiresAtUtc, string createdByIp, long userId)
        {
            Token = token;
            ExpiresAtUtc = expiresAtUtc;
            CreatedByIp = createdByIp;
            UserId = userId;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public long Id { get; private set; }

        public string Token { get; private set; } = null!;

        public long UserId { get; private set; }
        public User User { get; private set; } = null!;

        public DateTime ExpiresAtUtc { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        public string CreatedByIp { get; private set; } = null!;

        public DateTime? RevokedAtUtc { get; private set; }
        public string? RevokedByIp { get; private set; }
        public string? ReplacedByToken { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
        public bool IsRevoked => RevokedAtUtc.HasValue;
        public bool IsActive => !IsRevoked && !IsExpired;

        public void Revoke(string ip, string? replacedByToken = null)
        {
            RevokedAtUtc = DateTime.UtcNow;
            RevokedByIp = ip;
            ReplacedByToken = replacedByToken;
        }
    }
}
