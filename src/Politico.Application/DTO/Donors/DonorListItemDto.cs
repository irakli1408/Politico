namespace Politico.Application.DTO.Donors
{
    public sealed class DonorListItemDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? WebsiteUrl { get; set; }
        public string? LogoFilePath { get; set; }
    }
}
