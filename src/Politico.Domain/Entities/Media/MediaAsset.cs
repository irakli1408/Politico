namespace Politico.Domain.Entities.Media
{
    public enum MediaType
    {
        Photo = 1,
        Video = 2
    }
    public enum MediaOwnerType
    {
        Unknown = 0,
        Project = 1,
        Blog = 2,
        User = 3,
        // добавишь свои
    }
    public class MediaAsset
    {
        public long Id { get; set; }

        public MediaType Type { get; set; }

        public string OriginalFileName { get; set; } = default!;
        public string StoredPath { get; set; } = default!;
        public string? ThumbStoredPath { get; set; }
        public string? PosterPath { get; set; }

        public string ContentType { get; set; } = default!;
        public string Extension { get; set; } = default!;
        public long SizeBytes { get; set; }

        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? ThumbWidth { get; set; }
        public int? ThumbHeight { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        // корзина
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAtUtc { get; set; }

        public ICollection<MediaAttachment> Attachments { get; set; } = new List<MediaAttachment>();
    }
}
