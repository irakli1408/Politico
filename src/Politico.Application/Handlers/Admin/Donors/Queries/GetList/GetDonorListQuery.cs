using MediatR;
using Politico.Application.DTO.Donors;

namespace Politico.Application.Handlers.Admin.Donors.Queries.GetList
{
    public sealed record GetDonorListQuery(bool? OnlyActive)
    : IRequest<List<DonorListItemDto>>;
}
