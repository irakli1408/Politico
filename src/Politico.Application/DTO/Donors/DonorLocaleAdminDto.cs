namespace Politico.Application.DTO.Donors
{
    public sealed class DonorLocaleAdminDto
    {
        public int Id { get; set; }
        public string Culture { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
