namespace Politico.Domain.Entities.Donors
{
    public class DonorLocale
    {
        public int Id { get; set; }
        public int DonorId { get; set; }

        public string Culture { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public Donor Donor { get; set; } = default!;
    }
}
