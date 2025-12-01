using MediatR;
using Politico.Application.DTO.AboutOrg;

namespace Politico.Application.Features.AboutOrganization.Queries.GetAboutAdmin
{
    public sealed record GetAboutOrganizationQuery : IRequest<AboutOrganizationAdminDto>;
}
