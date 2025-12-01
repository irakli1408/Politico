using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Common.Enums.Media;
using Politico.Domain.Entities.Media;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service
{
    public sealed class DetachMediaService : IDetachMediaService
    {
        private readonly IAppDbContext _db;

        public DetachMediaService(IAppDbContext db) => _db = db;

        public async Task DetachAsync(long assetId, MediaOwnerType ownerType, string ownerKey, CancellationToken ct = default)
        {
            var attachment = await _db.MediaAttachments.FirstOrDefaultAsync(a =>
                a.MediaAssetId == assetId &&
                a.OwnerType == ownerType &&
                a.OwnerKey == ownerKey, ct);

            if (attachment is null)
                return;

            _db.MediaAttachments.Remove(attachment);
            await _db.SaveChangesAsync(ct);
        }
    }
}
