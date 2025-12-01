using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Politico.Application.Common.Helper.Model;
using Politico.Application.DTO.Donors;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Common.Enums.Media;

namespace Politico.Application.Handlers.Admin.Donors.Queries.GetList
{
    public sealed class GetDonorListHandler
        : IRequestHandler<GetDonorListQuery, List<DonorListItemDto>>
    {
        private readonly IAppDbContext _db;
        private readonly IOptions<MediaOptions> _mediaOpt;

        public GetDonorListHandler(IAppDbContext db, IOptions<MediaOptions> mediaOpt)
        {
            _db = db;
            _mediaOpt = mediaOpt;
        }

        public async Task<List<DonorListItemDto>> Handle(GetDonorListQuery q, CancellationToken ct)
        {
            const string defaultCulture = "ka";

            var baseQuery = _db.Donors
                .AsNoTracking()
                .AsQueryable();

            if (q.OnlyActive.HasValue)
                baseQuery = baseQuery.Where(d => d.IsActive == q.OnlyActive.Value);

            var rows = await baseQuery
                .Select(d => new
                {
                    d.Id,
                    d.IsActive,
                    d.WebsiteUrl,

                    Name = d.Locales
                        .Where(l => l.Culture == defaultCulture)
                        .Select(l => l.Name)
                        .FirstOrDefault()
                        ?? d.Locales
                            .Select(l => l.Name)
                            .FirstOrDefault(),

                    LogoStoredPath = _db.MediaAttachments
                        .Where(a =>
                            a.OwnerType == MediaOwnerType.Donors &&
                            a.OwnerKey == d.Id.ToString() &&
                            a.IsCover)
                        .Select(a => a.MediaAsset.StoredPath)
                        .FirstOrDefault()
                })
                .ToListAsync(ct);

            var list = rows
                .Select(x => new DonorListItemDto
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    WebsiteUrl = x.WebsiteUrl,
                    Name = x.Name ?? string.Empty,
                    LogoFilePath = MediaUrlHelper.ToUrl(_mediaOpt.Value, x.LogoStoredPath)
                })
                .ToList();

            return list;
        }
    }
}
