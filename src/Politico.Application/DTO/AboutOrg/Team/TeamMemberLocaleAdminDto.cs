using System;
using System.Collections.Generic;
using System.Text;

namespace Politico.Application.DTO.AboutOrg.Team
{
    public sealed class TeamMemberLocaleAdminDto
    {
        public string Culture { get; set; } = default!;  // "ka", "en"
        public string Name { get; set; } = default!;
        public string Position { get; set; } = default!;
        public string Bio { get; set; } = default!;
    }
}
