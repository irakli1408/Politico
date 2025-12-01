using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.Donors;

namespace Politico.Application.Handlers.Admin.Donors.Commands.Update
{
    public sealed class UpdateDonorHandler : IRequestHandler<UpdateDonorCommand, Unit>
    {
        private readonly IAppDbContext _db;

        public UpdateDonorHandler(IAppDbContext db) { _db = db; }

        public async Task<Unit> Handle(UpdateDonorCommand cmd, CancellationToken ct)
        {
            var dto = cmd.Donor;

            var entity = await _db.Donors
                .Include(d => d.Locales)
                .FirstOrDefaultAsync(d => d.Id == dto.Id, ct)
                ?? throw new Exception($"Donor {dto.Id} not found");

            entity.IsActive = dto.IsActive;
            entity.WebsiteUrl = dto.WebsiteUrl;

            var existingLocales = entity.Locales.ToDictionary(x => x.Id, x => x);

            foreach (var locDto in dto.Locales)
            {
                if (string.IsNullOrWhiteSpace(locDto.Name))
                    continue;

                if (locDto.Id > 0 && existingLocales.TryGetValue(locDto.Id, out var existing))
                {
                    existing.Culture = locDto.Culture;
                    existing.Name = locDto.Name;
                    existing.Description = locDto.Description;
                }
                else
                {
                    entity.Locales.Add(new DonorLocale
                    {
                        Culture = locDto.Culture,
                        Name = locDto.Name,
                        Description = locDto.Description
                    });
                }
            }

            // удаляем локали, которых нет в dto
            var dtoIds = dto.Locales
                .Where(l => l.Id > 0)
                .Select(l => l.Id)
                .ToHashSet();

            var toRemove = entity.Locales
                .Where(l => l.Id > 0 && !dtoIds.Contains(l.Id))
                .ToList();

            _db.Set<DonorLocale>().RemoveRange(toRemove);

            await _db.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
