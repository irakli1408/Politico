using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service;

public sealed class DeleteMediaService : IDeleteMediaService
{
    private readonly IAppDbContext _db;
    private readonly IFileStorage _storage;

    public DeleteMediaService(IAppDbContext db, IFileStorage storage)
    {
        _db = db;
        _storage = storage;
    }

    public async Task DeleteAsync(long assetId, CancellationToken ct = default)
    {
        var asset = await _db.MediaAssets
            .FirstOrDefaultAsync(x => x.Id == assetId, ct)
            ?? throw new InvalidOperationException("media_not_found");

        // 1) удалить основной файл
        if (!string.IsNullOrWhiteSpace(asset.StoredPath))
            await _storage.DeleteAsync(asset.StoredPath, ct);

        // 2) удалить превью, если есть
        if (!string.IsNullOrWhiteSpace(asset.ThumbStoredPath))
            await _storage.DeleteAsync(asset.ThumbStoredPath, ct);

        // 3) удалить привязки
        var attachments = _db.MediaAttachments.Where(a => a.MediaAssetId == assetId);
        _db.MediaAttachments.RemoveRange(attachments);

        // 4) удалить сам asset
        _db.MediaAssets.Remove(asset);

        await _db.SaveChangesAsync(ct);
    }
}
