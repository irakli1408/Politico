using Politico.FileManager.Common.DTO;

namespace Politico.FileManager.Common.Interfaces
{
    public interface IReorderMediaService
    {
        Task ReorderAsync(ReorderMediaRequest request, CancellationToken ct = default);
    }
}
