using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.DTO.Contact;
using Politico.Application.Interfaces.Persistence;

namespace Politico.Application.Handlers.Admin.Contact.Queries
{
    public sealed class GetContactAdminHandler
       : IRequestHandler<GetContactAdminQuery, ContactAdminDto>
    {
        private readonly IAppDbContext _db;

        // какие локали хотим поддерживать в админке
        private static readonly string[] _cultures = new[] { "ka", "en" };

        public GetContactAdminHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<ContactAdminDto> Handle(
            GetContactAdminQuery request,
            CancellationToken ct)
        {
            var entity = await _db.ContactInfos
                .Include(c => c.Locales)
                .FirstOrDefaultAsync(ct);

            if (entity is null)
            {
                // ещё нет записи — отдаём пустую форму
                return new ContactAdminDto
                {
                    Locales = _cultures
                        .Select(c => new ContactLocaleAdminDto
                        {
                            Culture = c,
                            Title = string.Empty,
                            Address = string.Empty,
                            WorkingHours = null,
                            AdditionalInfo = null
                        })
                        .ToList()
                };
            }

            return new ContactAdminDto
            {
                Phone1 = entity.Phone1,
                Phone2 = entity.Phone2,
                Email = entity.Email,
                TelegramLink = entity.TelegramLink,
                FacebookLink = entity.FacebookLink,
                InstagramLink = entity.InstagramLink,
                YoutubeLink = entity.YoutubeLink,
                Xlink = entity.Xlink,
                MapEmbedUrl = entity.MapEmbedUrl, 

                Locales = _cultures
                    .Select(culture =>
                    {
                        var loc = entity.Locales.FirstOrDefault(l => l.Culture == culture);

                        return new ContactLocaleAdminDto
                        {
                            Culture = culture,
                            Title = loc?.Title ?? string.Empty,
                            Address = loc?.Address ?? string.Empty,
                            WorkingHours = loc?.WorkingHours,
                            AdditionalInfo = loc?.AdditionalInfo
                        };
                    })
                    .ToList()
            };
        }
    }
}
