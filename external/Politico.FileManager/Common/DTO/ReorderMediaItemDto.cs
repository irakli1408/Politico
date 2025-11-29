using Politico.Domain.Entities.Media;

namespace Politico.FileManager.Common.DTO;

public sealed record ReorderMediaItemDto(
    long AssetId,
    int Order
);

public sealed record ReorderMediaRequest(
    MediaOwnerType OwnerType,
    string OwnerKey,
    IReadOnlyList<ReorderMediaItemDto> Items
);
