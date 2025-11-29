using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.Media;
using Politico.FileManager.Common.DTO;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service
{
    public sealed class GetOwnerMediaService : IGetOwnerMediaService
    {
        private readonly IAppDbContext _db;

        public GetOwnerMediaService(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<MediaItemDto>> GetByOwnerAsync(MediaOwnerType ownerType, string ownerKey, CancellationToken ct = default)
        {
            return await _db.MediaAttachments
                .Where(a => a.OwnerType == ownerType && a.OwnerKey == ownerKey)
                .OrderBy(a => a.Order)
                .Select(a => new MediaItemDto(
                    a.MediaAsset.Id,
                    a.MediaAsset.OriginalFileName,
                    a.MediaAsset.StoredPath,
                    a.MediaAsset.IsDeleted,
                    a.IsCover,
                    a.OwnerType,
                    a.OwnerKey,
                    a.Order))
                .ToListAsync(ct);
        }
    }
}
