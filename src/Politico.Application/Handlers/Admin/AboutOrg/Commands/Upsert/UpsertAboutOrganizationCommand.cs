using MediatR;
using Politico.Application.DTO.AboutOrg;

namespace Politico.Application.Handlers.Admin.AboutOrg.Commands.Upsert
{
    public sealed record UpsertAboutOrganizationACommand(
        AboutOrganizationDto Model) : IRequest<Unit>;
}
