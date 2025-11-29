namespace Politico.FileManager.Common.Options
{
    public class MediaOptions
    {
        public const string SectionName = "Media";

        public string RootPath { get; set; } = "uploads/media";
        public string RequestPath { get; set; } = "/media";

        public string[] AllowedImageExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".webp"];
        public string[] AllowedVideoExtensions { get; set; } = [".mp4", ".mov", ".avi", ".webm"];

        public int MaxImageSizeMb { get; set; } = 50;
        public int MaxVideoSizeMb { get; set; } = 5000;

        public int JpegQualityMain { get; set; } = 90;
        public int JpegQualityThumb { get; set; } = 80;
        public int ThumbMaxWidth { get; set; } = 480;
    }
}
