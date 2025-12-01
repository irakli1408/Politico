using Politico.Domain.Entities.Base;

namespace Politico.Domain.Entities.AboutOrg
{
    public class TeamMemberLocale : BaseEntity<int>
    {
        public int TeamMemberId { get; set; }
        public virtual TeamMember? TeamMember { get; set; }

        public string Culture { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Position { get; set; } = default!;

        public string Bio { get; set; } = default!;
    }
}
