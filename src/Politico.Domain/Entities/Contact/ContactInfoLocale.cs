using Politico.Domain.Entities.Base;

namespace Politico.Domain.Entities.Contact
{
    public sealed class ContactInfoLocale : BaseEntity<int>
    {
        public int ContactInfoId { get; set; }
        public ContactInfo? ContactInfo { get; set; }

        public string Culture { get; set; } = default!; // "ka", "en"

        // Заголовок секции, типа "Contact us" / "Контакты"
        public string Title { get; set; } = default!;

        public string? Address { get; set; } = default!;

        public string? WorkingHours { get; set; }

        public string? AdditionalInfo { get; set; }
    }
}
