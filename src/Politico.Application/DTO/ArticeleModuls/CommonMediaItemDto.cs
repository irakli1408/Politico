using Politico.Domain.Common.Enums.Media;

namespace Politico.Application.DTO.ArticeleModuls
{
    public sealed class CommonMediaItemDto
    {
        public long AssetId { get; set; }
        public string? Url { get; set; }
        public string? ThumbUrl { get; set; }
        public MediaType MediaType { get; set; }
        public int SortOrder { get; set; }
        public bool IsCover { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
