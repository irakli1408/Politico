using MediatR;
using Politico.Application.DTO.Donors;

namespace Politico.Application.Handlers.Admin.Donors.Queries.GetDeteils
{
    public sealed record GetDonorDetailsQuery(int Id)
    : IRequest<DonorAdminDto>;
}
