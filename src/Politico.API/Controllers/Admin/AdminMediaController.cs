using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Politico.API.RateLimiter;
using Politico.Domain.Entities.Media;
using Politico.FileManager.Common.DTO;
using Politico.FileManager.Common.Interfaces;

namespace Politico.API.Controllers.Admin
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/{culture}/admin/media")]
    public class AdminMediaController : ControllerBase
    {
        private readonly IUploadMediaService _uploadService;

        public AdminMediaController(IUploadMediaService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(2L * 1024 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 2L * 1024 * 1024 * 1024)]
        public async Task<IActionResult> Upload(
            [FromQuery(Name = "mediaTypes")] string type,
            IFormFile file,
            CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { error = "file_required" });

            MediaType mediaType = MediaType.Photo;
            if (int.TryParse(type, out var n) && Enum.IsDefined(typeof(MediaType), n))
                mediaType = (MediaType)n;
            else if (string.Equals(type, "video", StringComparison.OrdinalIgnoreCase))
                mediaType = MediaType.Video;

            var res = await _uploadService.UploadAsync(mediaType, file, ct);
            return Created(res.Url, res);
        }    

    [HttpDelete("{assetId:long}/trash")]
        public Task Trash(long assetId,
        [FromServices] ITrashMediaService service,
        CancellationToken ct)
        => service.TrashAsync(assetId, ct);

        [HttpPost("{assetId:long}/restore")]
        public Task Restore(long assetId,
            [FromServices] IRestoreMediaService service,
            CancellationToken ct)
            => service.RestoreAsync(assetId, ct);

        [HttpDelete("{assetId:long}")]
        public Task Delete(long assetId,
            [FromServices] IDeleteMediaService service,
            CancellationToken ct)
            => service.DeleteAsync(assetId, ct);


        [EnableRateLimiting("Strict3PerMinute")]
        [OutputCache(PolicyName = "CacheShort")]
        [HttpGet]
        public Task<IReadOnlyList<MediaItemDto>> Get([FromServices] IGetMediaListService service, CancellationToken ct)
            => service.GetAsync(ct);

        [HttpGet("trash")]
        public Task<IReadOnlyList<MediaItemDto>> GetTrash([FromServices] IGetTrashMediaListService service, CancellationToken ct)
            => service.GetTrashAsync(ct);

        [HttpPost("{ownerType}/{ownerKey}/{assetId:long}/attach")]
        public Task Attach(MediaOwnerType ownerType, string ownerKey, long assetId,
            [FromServices] IAttachMediaService service,
            CancellationToken ct)
            => service.AttachAsync(assetId, ownerType, ownerKey, ct);

        [HttpDelete("{ownerType}/{ownerKey}/{assetId:long}/detach")]
        public Task Detach(MediaOwnerType ownerType, string ownerKey, long assetId,
            [FromServices] IDetachMediaService service,
            CancellationToken ct)
            => service.DetachAsync(assetId, ownerType, ownerKey, ct);

        [HttpPost("{ownerType}/{ownerKey}/reorder")]
        public Task Reorder(MediaOwnerType ownerType, string ownerKey,
            [FromBody] IReadOnlyList<ReorderMediaItemDto> items,
            [FromServices] IReorderMediaService service,
            CancellationToken ct)
            => service.ReorderAsync(new ReorderMediaRequest(ownerType, ownerKey, items), ct);

        [HttpPost("{ownerType}/{ownerKey}/{assetId:long}/cover")]
        public Task SetCover(MediaOwnerType ownerType, string ownerKey, long assetId,
            [FromServices] ISetCoverMediaService service,
            CancellationToken ct)
            => service.SetCoverAsync(assetId, ownerType, ownerKey, ct);

        [HttpGet("{ownerType}/{ownerKey}")]
        public Task<IReadOnlyList<MediaItemDto>> GetOwnerMedia(MediaOwnerType ownerType, string ownerKey,
            [FromServices] IGetOwnerMediaService service,
            CancellationToken ct)
            => service.GetByOwnerAsync(ownerType, ownerKey, ct);

        [HttpGet("library")]
        public Task<IReadOnlyList<MediaItemDto>> GetLibrary(
            [FromServices] IGetMediaLibraryService service,
            CancellationToken ct)
            => service.GetLibraryAsync(ct);
    }
}
