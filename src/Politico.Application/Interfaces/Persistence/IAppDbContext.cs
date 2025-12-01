using Microsoft.EntityFrameworkCore;
using Politico.Domain.Entities.AboutOrg;
using Politico.Domain.Entities.Articels;
using Politico.Domain.Entities.Auth;
using Politico.Domain.Entities.Contact;
using Politico.Domain.Entities.Donors;
using Politico.Domain.Entities.ErrorLoger;
using Politico.Domain.Entities.Media;

namespace Politico.Application.Interfaces.Persistence
{
    public interface IAppDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; } 
        DbSet<UserRole> UserRoles { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<ErrorLog> ErrorLogs { get; }
        DbSet<MediaAsset> MediaAssets { get; }
        DbSet<MediaAttachment> MediaAttachments { get; }
        DbSet<Article> Articles { get; }
        DbSet<ArticleLocale> ArticleLocales { get; }
        DbSet<AboutOrganization> AboutOrganization { get; }
        DbSet<AboutOrganizationLocale> AboutOrganizationLocales { get; }
        DbSet<TeamMember> TeamMembers { get; }
        DbSet<TeamMemberLocale> TeamMemberLocales { get; }
        DbSet<ContactInfo> ContactInfos { get; }
        DbSet<ContactInfoLocale> ContactInfoLocales { get; }
        DbSet<Donor> Donors { get; }
        DbSet<DonorLocale> DonorLocale { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
