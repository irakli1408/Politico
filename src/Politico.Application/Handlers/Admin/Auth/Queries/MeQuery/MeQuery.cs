using MediatR;
using Politico.Application.DTO.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Queries.MeQuery
{
    public sealed record MeQuery : IRequest<UserDto>;
}
