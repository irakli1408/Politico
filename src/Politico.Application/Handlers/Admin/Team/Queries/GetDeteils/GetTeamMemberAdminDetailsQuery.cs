using MediatR;
using Politico.Application.DTO.AboutOrg.Team;

namespace Politico.Application.Handlers.Admin.Team.Queries.GetDeteils
{
    public sealed record GetTeamMemberAdminDetailsQuery(int Id) : IRequest<TeamMemberAdminDetailsDto>;
}
