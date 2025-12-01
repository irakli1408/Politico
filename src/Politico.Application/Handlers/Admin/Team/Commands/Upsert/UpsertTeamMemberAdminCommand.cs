using MediatR;
using Politico.Application.DTO.AboutOrg.Team;

namespace Politico.Application.Handlers.Admin.Team.Commands.Upsert
{
    public sealed record UpsertTeamMemberAdminCommand(TeamMemberUpsertDto Model)
    : IRequest<int>;
}
