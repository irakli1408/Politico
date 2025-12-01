namespace Politico.Application.DTO.AboutOrg.Team
{
    public sealed class TeamMemberUpsertDto
    {
        // 0 или null = создать нового; >0 = обновить
        public int Id { get; set; }

        public bool IsActive { get; set; } = true;
        public int OrderIndex { get; set; }

        public List<TeamMemberLocaleAdminDto> Locales { get; set; } = new();
    }
}
