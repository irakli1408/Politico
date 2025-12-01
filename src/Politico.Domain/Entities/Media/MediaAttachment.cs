using Politico.Domain.Common.Enums.Media;

namespace Politico.Domain.Entities.Media
{
    public class MediaAttachment
    {
        public long Id { get; set; }

        public long MediaAssetId { get; set; }
        public MediaAsset MediaAsset { get; set; } = default!;

        public MediaOwnerType OwnerType { get; set; }
        public string OwnerKey { get; set; } = default!; 

        public int Order { get; set; }
        public bool IsCover { get; set; }
    }
}
