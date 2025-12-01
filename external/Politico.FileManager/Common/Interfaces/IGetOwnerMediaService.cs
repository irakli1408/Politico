using Politico.Domain.Common.Enums.Media;
using Politico.Domain.Entities.Media;
using Politico.FileManager.Common.DTO;

namespace Politico.FileManager.Common.Interfaces
{
    public interface IGetOwnerMediaService
    {
        Task<IReadOnlyList<MediaItemDto>> GetByOwnerAsync(MediaOwnerType ownerType, string ownerKey, CancellationToken ct = default);
    }
}
