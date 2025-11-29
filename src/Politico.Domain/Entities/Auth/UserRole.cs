namespace Politico.Domain.Entities.Auth
{
    public class UserRole
    {
        private UserRole() { }

        public UserRole(User user, Role role)
        {
            User = user;
            Role = role;
            UserId = user.Id;
            RoleId = role.Id;
        }

        public long UserId { get; private set; }
        public User User { get; private set; } = null!;

        public long RoleId { get; private set; }
        public Role Role { get; private set; } = null!;
    }
}
