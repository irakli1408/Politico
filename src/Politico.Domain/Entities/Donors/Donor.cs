using Politico.Domain.Common.Interfaces;
using Politico.Domain.Entities.Base;

namespace Politico.Domain.Entities.Donors
{
    public class Donor : BaseEntity<int>, IHasId<int>, IActivatable
    {
        public bool IsActive { get; set; }            
        public string? WebsiteUrl { get; set; }
        public string? LogoFilePath { get; set; }
        public List<DonorLocale> Locales { get; set; } = new();

    }
}
