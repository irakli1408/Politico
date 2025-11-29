using Microsoft.AspNetCore.Http;
using Politico.Domain.Entities.Media;
using Politico.FileManager.Common.DTO;

namespace Politico.FileManager.Common.Interfaces
{
    public interface IUploadMediaService
    {
        Task<UploadMediaResult> UploadAsync(MediaType type, IFormFile file, CancellationToken ct = default);
    }
}
