using MediatR;

namespace Politico.Application.Handlers.Admin.Team.Commands.HardDelete
{
    public sealed record DeleteTeamMemberAdminCommand(int Id) : IRequest<Unit>;
}
