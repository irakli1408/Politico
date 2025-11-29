namespace Politico.FileManager.Common.Interfaces
{
    public interface IFileStorage
    {
        Task<string> SaveAsync(Stream fileStream, string subfolder, string fileName, CancellationToken ct = default); 
       
        Task DeleteAsync(string relativePath, CancellationToken ct = default);
    }
}
