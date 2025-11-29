using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.FileManager.Common.DTO;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service
{
    public sealed class ReorderMediaService : IReorderMediaService
    {
        private readonly IAppDbContext _db;

        public ReorderMediaService(IAppDbContext db) => _db = db;

        public async Task ReorderAsync(ReorderMediaRequest request, CancellationToken ct = default)
        {
            var attachments = await _db.MediaAttachments
                .Where(a => a.OwnerType == request.OwnerType && a.OwnerKey == request.OwnerKey)
                .ToListAsync(ct);

            foreach (var item in request.Items)
            {
                var attach = attachments.FirstOrDefault(a => a.MediaAssetId == item.AssetId);
                if (attach is not null)
                    attach.Order = item.Order;
            }

            await _db.SaveChangesAsync(ct);
        }
    }
}
