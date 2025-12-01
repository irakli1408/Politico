namespace Politico.Application.DTO.AboutOrg.Team
{
    public sealed class TeamMemberListItemAdminDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int OrderIndex { get; set; }

        public string Name { get; set; } = default!;
        public string Position { get; set; } = default!;

        public string? PhotoUrl { get; set; }
    }
}
