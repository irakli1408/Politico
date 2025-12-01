namespace Politico.Application.DTO.Contact
{
    public sealed class ContactAdminDto
    {
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }

        public string? TelegramLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? YoutubeLink { get; set; }
        public string? Xlink { get; set; }

        public string? MapEmbedUrl { get; set; }

        // Локали
        public List<ContactLocaleAdminDto> Locales { get; set; } = new();
    }
}
