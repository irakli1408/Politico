using Politico.Domain.Entities.Base;

namespace Politico.Domain.Entities.AboutOrg;

public class AboutOrganizationLocale : BaseEntity<int>
{
    public int AboutOrganizationId { get; set; }
    public AboutOrganization? AboutOrganization { get; set; }

    public string Culture { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
}
