using Politico.Domain.Entities.Base;

namespace Politico.Domain.Entities.Contact
{
    public sealed class ContactInfo : BaseEntity<int>
    {
        // Телефоны / email — общие для всех языков
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }

        // Соцсети / мессенджеры
        public string? TelegramLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? YoutubeLink { get; set; }
        public string? Xlink { get; set; }
       

        // Карта — например, embed-url Google Maps / OpenStreetMap
        public string? MapEmbedUrl { get; set; }

        public ICollection<ContactInfoLocale> Locales { get; set; }
            = new List<ContactInfoLocale>();
    }
}
