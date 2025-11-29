using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.Media;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service
{
    public sealed class SetCoverMediaService : ISetCoverMediaService
    {
        private readonly IAppDbContext _db;

        public SetCoverMediaService(IAppDbContext db) => _db = db;

        public async Task SetCoverAsync(long assetId, MediaOwnerType ownerType, string ownerKey, CancellationToken ct = default)
        {
            var attachments = await _db.MediaAttachments
                .Where(a => a.OwnerType == ownerType && a.OwnerKey == ownerKey)
                .ToListAsync(ct);

            foreach (var a in attachments)
                a.IsCover = a.MediaAssetId == assetId;

            await _db.SaveChangesAsync(ct);
        }
    }

}
