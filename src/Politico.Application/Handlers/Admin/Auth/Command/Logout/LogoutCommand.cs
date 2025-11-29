using MediatR;

namespace Politico.Application.Handlers.Admin.Auth.Command.Logout
{
    public sealed record LogoutCommand(string RefreshToken) : IRequest<Unit>;
}
