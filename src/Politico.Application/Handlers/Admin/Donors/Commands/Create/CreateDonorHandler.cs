using MediatR;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.Donors;

namespace Politico.Application.Handlers.Admin.Donors.Commands.Create
{
    public sealed class CreateDonorHandler : IRequestHandler<CreateDonorCommand, int>
    {
        private readonly IAppDbContext _db;

        public CreateDonorHandler(IAppDbContext db) { _db = db; }

        public async Task<int> Handle(CreateDonorCommand cmd, CancellationToken ct)
        {
            var dto = cmd.Donor;

            var entity = new Donor
            {
                IsActive = dto.IsActive,
                WebsiteUrl = dto.WebsiteUrl,
                LogoFilePath = dto.LogoFilePath
            };

            foreach (var locDto in dto.Locales)
            {
                if (string.IsNullOrWhiteSpace(locDto.Name))
                    continue;

                entity.Locales.Add(new DonorLocale
                {
                    Culture = locDto.Culture,
                    Name = locDto.Name,
                    Description = locDto.Description
                });
            }

            _db.Donors.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity.Id;
        }
    }
}
