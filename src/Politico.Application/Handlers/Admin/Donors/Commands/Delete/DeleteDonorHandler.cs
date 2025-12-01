using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Exceptions;

namespace Politico.Application.Handlers.Admin.Donors.Commands.Delete
{
    public sealed class DeleteDonorHandler : IRequestHandler<DeleteDonorCommand>
    {
        private readonly IAppDbContext _db;

        public DeleteDonorHandler(IAppDbContext db) { _db = db; }

        public async Task Handle(DeleteDonorCommand cmd, CancellationToken ct)
        {
            var donor = await _db.Donors
                .FirstOrDefaultAsync(d => d.Id == cmd.Id, ct);

            if (donor is null)
                throw new NotFoundException("Donor not found.");

            donor.DeleteDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
        }
    }
}