using MediatR;

namespace Politico.Application.Handlers.Admin.Auth.Command.MakeAdmin
{
    public sealed record MakeAdminCommand(long UserId) : IRequest<Unit>;
}
