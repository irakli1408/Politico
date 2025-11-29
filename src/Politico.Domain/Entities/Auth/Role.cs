namespace Politico.Domain.Entities.Auth
{
    public class Role
    {
        private Role() { }

        public Role(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
        public Role(long id, string name, string? description = null)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public long Id { get; private set; }

        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles;
    }

    public static class RoleNames
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
