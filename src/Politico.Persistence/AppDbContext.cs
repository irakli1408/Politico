using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.AboutOrg;
using Politico.Domain.Entities.Articels;
using Politico.Domain.Entities.Auth;
using Politico.Domain.Entities.Contact;
using Politico.Domain.Entities.Donors;
using Politico.Domain.Entities.ErrorLoger;
using Politico.Domain.Entities.Media;
using Politico.Persistence.Configuration.Auth.Seed;

namespace Politico.Persistence;

public class AppDbContext : DbContext, IAppDbContext

{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public new DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>(); 
    public DbSet<MediaAsset> MediaAssets => Set<MediaAsset>();
    public DbSet<MediaAttachment> MediaAttachments => Set<MediaAttachment>();
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<ArticleLocale> ArticleLocales { get; set; } = null!;
    public DbSet<AboutOrganization> AboutOrganization { get; set; } = null!;
    public DbSet<AboutOrganizationLocale> AboutOrganizationLocales { get; set; } = null!;
    public DbSet<TeamMember> TeamMembers { get; set; } = null!;
    public DbSet<TeamMemberLocale> TeamMemberLocales { get; set; } = null!; 
    public DbSet<ContactInfo> ContactInfos { get; set; }
    public DbSet<ContactInfoLocale> ContactInfoLocales { get; set; }
    public DbSet<Donor> Donors { get; set; }
    public DbSet<DonorLocale> DonorLocale { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        IdentityRoleSeeder.Seed(modelBuilder);
        //modelBuilder.ApplyConfiguration(new UserConfiguration());
        //modelBuilder.ApplyConfiguration(new RoleConfiguration());
        //modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        //modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        //modelBuilder.ApplyConfiguration(new MediaAssetConfig());
        //modelBuilder.ApplyConfiguration(new MediaAttachmentConfig());
        //modelBuilder.ApplyConfiguration(new ArticleConfig());
        //modelBuilder.ApplyConfiguration(new ArticleLocaleConfig());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
