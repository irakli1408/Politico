using Politico.Application.DTO.ArticeleModuls;

namespace Politico.Application.DTO.AboutOrg.Team
{
    public sealed class TeamMemberAdminDetailsDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int OrderIndex { get; set; }

        public List<TeamMemberLocaleAdminDto> Locales { get; set; } = new();

        public long? CoverId { get; set; }
        public List<CommonMediaItemDto> Media { get; set; } = new();
    }
}
