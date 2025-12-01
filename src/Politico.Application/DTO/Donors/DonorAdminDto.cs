namespace Politico.Application.DTO.Donors
{
    public sealed class DonorAdminDto
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }= false;

        public string? WebsiteUrl { get; set; }
        public string? LogoFilePath { get; set; }

        public List<DonorLocaleAdminDto> Locales { get; set; } = new();
    }
}
