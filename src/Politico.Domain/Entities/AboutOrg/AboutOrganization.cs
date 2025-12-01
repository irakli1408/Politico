using Politico.Domain.Entities.Base;

namespace Politico.Domain.Entities.AboutOrg
{
    public class AboutOrganization : BaseEntity<int>
    {
      public virtual ICollection<AboutOrganizationLocale> Locales { get; set; } = new List<AboutOrganizationLocale>();
    }
}
