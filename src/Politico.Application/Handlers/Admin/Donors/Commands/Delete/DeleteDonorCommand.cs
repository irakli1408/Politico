using MediatR;

namespace Politico.Application.Handlers.Admin.Donors.Commands.Delete
{
    public sealed record DeleteDonorCommand(int Id) : IRequest;
}
