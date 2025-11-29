using Microsoft.EntityFrameworkCore;
using Politico.Domain.Entities.Auth;

namespace Politico.Persistence.Configuration.Auth.Seed
{
    public static class IdentityRoleSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role(1, RoleNames.SuperAdmin),
                new Role(2, RoleNames.Admin),
                new Role(3, RoleNames.User)
            );
        }
    }
}
