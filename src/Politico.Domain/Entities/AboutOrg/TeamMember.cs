using Politico.Domain.Common.Interfaces;
using Politico.Domain.Entities.Base;

namespace Politico.Domain.Entities.AboutOrg
{
    public class TeamMember : BaseEntity<int>, IHasId<int>, IActivatable
    {
        public bool IsActive { get; set; } = true;
        public int OrderIndex { get; set; }
        public virtual ICollection<TeamMemberLocale> Locales { get; set; }
            = new List<TeamMemberLocale>();
    }
}
