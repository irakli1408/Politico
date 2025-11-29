namespace Politico.FileManager.Common.Interfaces
{
    public interface IDeleteMediaService
    {
        Task DeleteAsync(long assetId, CancellationToken ct = default);
    }
}
