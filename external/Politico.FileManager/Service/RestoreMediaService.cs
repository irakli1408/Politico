using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service
{
    public sealed class RestoreMediaService : IRestoreMediaService
    {
        private readonly IAppDbContext _db;

        public RestoreMediaService(IAppDbContext db) => _db = db;

        public async Task RestoreAsync(long assetId, CancellationToken ct = default)
        {
            var asset = await _db.MediaAssets.FirstOrDefaultAsync(x => x.Id == assetId, ct)
                        ?? throw new InvalidOperationException("Asset not found");

            asset.IsDeleted = false;
            await _db.SaveChangesAsync(ct);
        }
    }
}
