namespace Politico.FileManager.Common.Interfaces
{
    public interface ITrashMediaService
    {
        Task TrashAsync(long assetId, CancellationToken ct = default);
    }
}
