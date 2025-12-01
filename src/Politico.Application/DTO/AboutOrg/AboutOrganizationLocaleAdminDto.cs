namespace Politico.Application.DTO.AboutOrg
{
    public sealed class AboutOrganizationLocaleAdminDto
    {
        public string Culture { get; set; } = default!; // "ka", "en"
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
}
