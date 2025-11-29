namespace Politico.FileManager.Common.Interfaces
{
    public interface IRestoreMediaService
    {
        Task RestoreAsync(long assetId, CancellationToken ct = default);
    }
}
