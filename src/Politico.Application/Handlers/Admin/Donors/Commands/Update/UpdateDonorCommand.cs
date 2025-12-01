using MediatR;
using Politico.Application.DTO.Donors;

namespace Politico.Application.Handlers.Admin.Donors.Commands.Update
{
    public sealed class UpdateDonorCommand : IRequest<Unit>
    {
        public DonorAdminDto Donor { get; set; } = default!;
    }
}
