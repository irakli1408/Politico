using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service
{
    public sealed class TrashMediaService : ITrashMediaService
    {
        private readonly IAppDbContext _db;

        public TrashMediaService(IAppDbContext db) => _db = db;

        public async Task TrashAsync(long assetId, CancellationToken ct = default)
        {
            var asset = await _db.MediaAssets.FirstOrDefaultAsync(x => x.Id == assetId, ct)
                        ?? throw new InvalidOperationException("Asset not found");

            asset.IsDeleted = true;
            await _db.SaveChangesAsync(ct);
        }
    }
}
