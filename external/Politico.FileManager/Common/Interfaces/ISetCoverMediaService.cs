using Politico.Domain.Entities.Media;

namespace Politico.FileManager.Common.Interfaces
{
    public interface ISetCoverMediaService
    {
        Task SetCoverAsync(long assetId, MediaOwnerType ownerType, string ownerKey, CancellationToken ct = default);
    }
}
