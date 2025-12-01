namespace Politico.Application.DTO.Contact
{
    public sealed class ContactLocaleAdminDto
    {
        public string Culture { get; set; } = default!;  // "ka", "en"
        public string Title { get; set; } = default!;    // "Contact us", "Контакты"
        public string? Address { get; set; } = default!;
        public string? WorkingHours { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
