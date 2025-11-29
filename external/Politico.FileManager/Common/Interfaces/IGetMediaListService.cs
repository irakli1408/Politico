using Politico.FileManager.Common.DTO;

namespace Politico.FileManager.Common.Interfaces
{
    public interface IGetMediaListService
    {
        Task<IReadOnlyList<MediaItemDto>> GetAsync(CancellationToken ct = default);
    }
}
