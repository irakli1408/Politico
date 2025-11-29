namespace Politico.Domain.Entities.Auth
{
    public class User
    {
        private User() { }

        public User(string email, string userName, string passwordHash)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            IsActive = true;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public long Id { get; private set; }

        public string Email { get; private set; } = null!;
        public string UserName { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public bool IsActive { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? UpdatedAtUtc { get; private set; }

        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles;

        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

        #region Logic

        public void ChangePasswordHash(string hash)
        {
            PasswordHash = hash;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Block()
        {
            if (!IsActive) return;
            IsActive = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Unblock()
        {
            if (IsActive) return;
            IsActive = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public bool HasRole(string roleName) =>
            _userRoles.Any(x => x.Role.Name == roleName);

        public bool IsSuperAdmin() =>
            HasRole(RoleNames.SuperAdmin);

        public void AssignRole(Role role)
        {
            if (_userRoles.Any(x => x.RoleId == role.Id))
                return;

            _userRoles.Add(new UserRole(this, role));
        }

        public void RemoveRole(Role role)
        {
            var existing = _userRoles.FirstOrDefault(x => x.RoleId == role.Id);
            if (existing != null)
                _userRoles.Remove(existing);
        }

        public void AddRefreshToken(RefreshToken token)
        {
            _refreshTokens.Add(token);
        }

        #endregion
    }
}
