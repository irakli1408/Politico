using MediatR;
using Politico.Application.DTO.AboutOrg.Team;

namespace Politico.Application.Handlers.Admin.Team.Queries.GetList
{
    public sealed record GetTeamMemberAdminListQuery(bool OnlyActive)
        : IRequest<List<TeamMemberListItemAdminDto>>;
}
