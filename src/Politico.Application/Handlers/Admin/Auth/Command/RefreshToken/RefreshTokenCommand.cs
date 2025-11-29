using MediatR;
using Politico.Application.DTO.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.RefreshToken
{
    public sealed record RefreshTokenCommand(string RefreshToken)
         : IRequest<AuthResultDto>;
}
