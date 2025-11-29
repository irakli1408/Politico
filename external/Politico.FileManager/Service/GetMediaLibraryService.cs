using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.FileManager.Common.DTO;
using Politico.FileManager.Common.Interfaces;

namespace Politico.FileManager.Service
{
    public sealed class GetMediaLibraryService : IGetMediaLibraryService
    {
        private readonly IAppDbContext _db;

        public GetMediaLibraryService(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<MediaItemDto>> GetLibraryAsync(CancellationToken ct = default)
        {
            // пока просто все не удалённые – потом добавишь пагинацию/фильтры
            return await _db.MediaAssets
                .Where(x => !x.IsDeleted)
                .Select(x => new MediaItemDto(
                    x.Id,
                    x.OriginalFileName,
                    x.StoredPath,
                    x.IsDeleted,
                    false,
                    null,
                    null,
                    0))
                .ToListAsync(ct);
        }
    }
}
