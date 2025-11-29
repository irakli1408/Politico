using Politico.FileManager.Common.DTO;

namespace Politico.FileManager.Common.Interfaces
{
    public interface IGetTrashMediaListService
    {
        Task<IReadOnlyList<MediaItemDto>> GetTrashAsync(CancellationToken ct = default);
    }
}
