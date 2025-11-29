using Microsoft.EntityFrameworkCore;
using Politico.Domain.Entities.Auth;
using Politico.Domain.Entities.ErrorLoger;
using Politico.Domain.Entities.Media;

namespace Politico.Application.Interfaces.Persistence
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; } 
        DbSet<UserRole> UserRoles { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<ErrorLog> ErrorLogs { get; }
        DbSet<MediaAsset> MediaAssets { get; }
        DbSet<MediaAttachment> MediaAttachments { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
