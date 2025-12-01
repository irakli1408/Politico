using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Politico.Application.Common.Helper.Model;
using Politico.Application.DTO.AboutOrg;
using Politico.Application.DTO.ArticeleModuls; 
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Common.Enums.Media;

namespace Politico.Application.Features.AboutOrganization.Queries.GetAboutAdmin
{
    public sealed class GetAboutOrganizationHandler
        : IRequestHandler<GetAboutOrganizationQuery, AboutOrganizationAdminDto>
    {
        private readonly IAppDbContext _db;
        private readonly MediaOptions _mediaOpt;

        private static readonly string[] _cultures = new[] { "ka", "en" };

        public GetAboutOrganizationHandler(
            IAppDbContext db,
            IOptions<MediaOptions> mediaOpt)
        {
            _db = db;
            _mediaOpt = mediaOpt.Value;
        }

        public async Task<AboutOrganizationAdminDto> Handle(
            GetAboutOrganizationQuery request,
            CancellationToken ct)
        {
            // 1) AboutOrganization + локали
            var about = await _db.AboutOrganization
                .Include(a => a.Locales)
                .FirstOrDefaultAsync(ct);

            if (about is null)
            {
                return new AboutOrganizationAdminDto
                {
                    Locales = _cultures
                        .Select(c => new AboutOrganizationLocaleAdminDto
                        {
                            Culture = c,
                            Title = string.Empty,
                            Content = string.Empty
                        })
                        .ToList(),
                    CoverId = null,
                    Media = new List<CommonMediaItemDto>()
                };
            }

            // 2) Медиа для AboutOrganization
            var attachments = await _db.MediaAttachments
                .Include(m => m.MediaAsset)
                .Where(m =>
                    m.OwnerType == MediaOwnerType.AboutOrganization &&
                    m.OwnerKey == about.Id.ToString())
                .OrderBy(m => m.Order)
                .ToListAsync(ct);

            var mediaDtos = attachments.Select(m => new CommonMediaItemDto
            {
                AssetId = m.MediaAssetId,
                Url = m.MediaAsset?.StoredPath is null
                    ? null
                    : MediaUrlHelper.ToUrl(_mediaOpt, m.MediaAsset.StoredPath),
                ThumbUrl = m.MediaAsset?.ThumbStoredPath is null
                    ? null
                    : MediaUrlHelper.ToUrl(_mediaOpt, m.MediaAsset.ThumbStoredPath),
                MediaType = m.MediaAsset?.Type ?? MediaType.Unknown,
                SortOrder = m.Order,
                IsCover = m.IsCover,
                CreatedAtUtc = m.MediaAsset?.CreatedAtUtc ?? DateTime.MinValue
            }).ToList();

            var coverId = mediaDtos.FirstOrDefault(x => x.IsCover)?.AssetId;

            // 3) Собираем DTO
            return new AboutOrganizationAdminDto
            {
                Locales = _cultures
                    .Select(culture =>
                    {
                        var loc = about.Locales.FirstOrDefault(l => l.Culture == culture);

                        return new AboutOrganizationLocaleAdminDto
                        {
                            Culture = culture,
                            Title = loc?.Title ?? string.Empty,
                            Content = loc?.Content ?? string.Empty
                        };
                    })
                    .ToList(),

                CoverId = coverId,
                Media = mediaDtos
            };
        }
    }
}
