using Politico.FileManager.Common.DTO;

namespace Politico.FileManager.Common.Interfaces
{
    public interface IGetMediaLibraryService
    {
        Task<IReadOnlyList<MediaItemDto>> GetLibraryAsync(CancellationToken ct = default);
    }
}
