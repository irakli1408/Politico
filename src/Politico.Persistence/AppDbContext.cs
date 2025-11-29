using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.Auth;
using Politico.Domain.Entities.ErrorLoger;
using Politico.Domain.Entities.Media;
using Politico.Persistence.Configuration.Auth;
using Politico.Persistence.Configuration.Auth.Seed;
using Politico.Persistence.Configuration.Media;

namespace Politico.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>(); 
    public DbSet<MediaAsset> MediaAssets => Set<MediaAsset>();
    public DbSet<MediaAttachment> MediaAttachments => Set<MediaAttachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        IdentityRoleSeeder.Seed(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new MediaAssetConfig());
        modelBuilder.ApplyConfiguration(new MediaAttachmentConfig());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
