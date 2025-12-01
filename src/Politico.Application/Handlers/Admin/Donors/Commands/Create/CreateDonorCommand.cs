using MediatR;
using Politico.Application.DTO.Donors;

namespace Politico.Application.Handlers.Admin.Donors.Commands.Create
{
    public sealed class CreateDonorCommand : IRequest<int>
    {
        public DonorAdminDto Donor { get; set; } = default!;
    }
}
