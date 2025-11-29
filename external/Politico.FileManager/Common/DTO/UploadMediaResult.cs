namespace Politico.FileManager.Common.DTO
{
    public record UploadMediaResult(
    long AssetId,
    string Name,
    int MediaType,
    string Url,
    string? ThumbUrl
);
}
