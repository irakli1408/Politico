using Politico.Domain.Common.Enums.Media;

namespace Politico.FileManager.Common.DTO
{
    public sealed record MediaItemDto(
     long Id,
     string FileName,
     string Url,
     bool IsInTrash,
     bool IsCover,
     MediaOwnerType? OwnerType,
     string? OwnerKey,
     int Order
 );
}
