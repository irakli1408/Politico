using MediatR;
using Politico.Application.DTO.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.Register
{
    public sealed record RegisterCommand(string Email, string UserName, string Password, bool IsAdmin)
    : IRequest<UserDto>;
}
