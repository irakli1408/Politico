using MediatR;
using Politico.Application.DTO.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.Login
{
    public sealed record LoginCommand(string Email, string Password)
       : IRequest<AuthResultDto>;
}
