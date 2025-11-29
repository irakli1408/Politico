using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.Media;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service
{
    public sealed class AttachMediaService : IAttachMediaService
    {
        private readonly IAppDbContext _db;

        public AttachMediaService(IAppDbContext db) => _db = db;

        public async Task AttachAsync(long assetId, MediaOwnerType ownerType, string ownerKey, CancellationToken ct = default)
        {
            var assetExists = await _db.MediaAssets.AnyAsync(x => x.Id == assetId && !x.IsDeleted, ct);
            if (!assetExists)
                throw new InvalidOperationException("Asset not found");

            var already = await _db.MediaAttachments.AnyAsync(a =>
                a.MediaAssetId == assetId &&
                a.OwnerType == ownerType &&
                a.OwnerKey == ownerKey, ct);

            if (!already)
            {
                var order = await _db.MediaAttachments
                    .Where(a => a.OwnerType == ownerType && a.OwnerKey == ownerKey)
                    .MaxAsync(a => (int?)a.Order, ct) ?? 0;

                _db.MediaAttachments.Add(new MediaAttachment
                {
                    MediaAssetId = assetId,
                    OwnerType = ownerType,
                    OwnerKey = ownerKey,
                    Order = order + 1,
                    IsCover = false
                });

                await _db.SaveChangesAsync(ct);
            }
        }
    }
}
