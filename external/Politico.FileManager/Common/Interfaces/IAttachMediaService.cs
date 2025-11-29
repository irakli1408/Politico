using Politico.Domain.Entities.Media;

namespace Politico.FileManager.Common.Interfaces
{
    public interface IAttachMediaService
    {
        Task AttachAsync(long assetId, MediaOwnerType ownerType, string ownerKey, CancellationToken ct = default);
    }
}
