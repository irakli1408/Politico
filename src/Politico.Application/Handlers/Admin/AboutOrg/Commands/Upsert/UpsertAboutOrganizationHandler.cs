using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.AboutOrg;

namespace Politico.Application.Handlers.Admin.AboutOrg.Commands.Upsert
{
    public sealed class UpsertAboutOrganizationHandler
        : IRequestHandler<UpsertAboutOrganizationACommand, Unit>
    {
        private readonly IAppDbContext _db;

        public UpsertAboutOrganizationHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(
            UpsertAboutOrganizationACommand request,
            CancellationToken ct)
        {
            var dto = request.Model;

            var entity = await _db.AboutOrganization
                .Include(x => x.Locales)
                .FirstOrDefaultAsync(ct);

            // если записи ещё нет — создаём
            if (entity is null)
            {
                entity = new AboutOrganization();
                _db.AboutOrganization.Add(entity);
            }

            foreach (var localeDto in dto.Locales)
            {
                var locale = entity.Locales
                    .FirstOrDefault(l => l.Culture == localeDto.Culture);

                if (locale is null)
                {
                    locale = new AboutOrganizationLocale
                    {
                        Culture = localeDto.Culture,
                        Title = localeDto.Title,
                        Content = localeDto.Content
                    };

                    entity.Locales.Add(locale);
                }
                else
                {
                    locale.Title = localeDto.Title;
                    locale.Content = localeDto.Content;
                }
            }

            await _db.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
