using Politico.Domain.Common.Enums.Media;
using Politico.Domain.Entities.Media;

namespace Politico.FileManager.Common.Interfaces
{
    public interface IDetachMediaService
    {
        Task DetachAsync(long assetId, MediaOwnerType ownerType, string ownerKey, CancellationToken ct = default);
    }
}
