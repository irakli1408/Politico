using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.Contact;

namespace Politico.Application.Handlers.Admin.Contact.Commands.Upsert
{
    public sealed class UpsertContactAdminHandler : IRequestHandler<UpsertContactAdminCommand,Unit>
    {
        private readonly IAppDbContext _db;

        public UpsertContactAdminHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(
            UpsertContactAdminCommand request,
            CancellationToken ct)
        {
            var dto = request.Model;

            var entity = await _db.ContactInfos
                .Include(c => c.Locales)
                .FirstOrDefaultAsync(ct);

            if (entity is null)
            {
                entity = new ContactInfo();
                _db.ContactInfos.Add(entity);
            }

            entity.Phone1 = dto.Phone1;
            entity.Phone2 = dto.Phone2;
            entity.Email = dto.Email;
            entity.TelegramLink = dto.TelegramLink;
            entity.FacebookLink = dto.FacebookLink;
            entity.InstagramLink = dto.InstagramLink;
            entity.YoutubeLink = dto.YoutubeLink;
            entity.Xlink = dto.Xlink;
            entity.MapEmbedUrl = dto.MapEmbedUrl;

            foreach (var localeDto in dto.Locales)
            {
                var loc = entity.Locales
                    .FirstOrDefault(l => l.Culture == localeDto.Culture);

                if (loc is null)
                {
                    loc = new ContactInfoLocale
                    {
                        Culture = localeDto.Culture,
                        Title = localeDto.Title,
                        Address = localeDto.Address,
                        WorkingHours = localeDto.WorkingHours,
                        AdditionalInfo = localeDto.AdditionalInfo
                    };
                    entity.Locales.Add(loc);
                }
                else
                {
                    loc.Title = localeDto.Title;
                    loc.Address = localeDto.Address;
                    loc.WorkingHours = localeDto.WorkingHours;
                    loc.AdditionalInfo = localeDto.AdditionalInfo;
                }
            }

            await _db.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
