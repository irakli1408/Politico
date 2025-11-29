using MediatR;

namespace Politico.Application.Handlers.Admin.Auth.Command.RemoveAdmin
{
    public sealed record RemoveAdminCommand(long UserId) : IRequest<Unit>;
}
