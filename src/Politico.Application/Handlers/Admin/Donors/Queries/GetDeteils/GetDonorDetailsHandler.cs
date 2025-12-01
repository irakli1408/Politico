using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Politico.Application.Common.Helper.Model;
using Politico.Application.DTO.Donors;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Exceptions;
using Politico.Domain.Common.Enums.Media;

namespace Politico.Application.Handlers.Admin.Donors.Queries.GetDeteils
{
    public sealed class GetDonorDetailsHandler
        : IRequestHandler<GetDonorDetailsQuery, DonorAdminDto>
    {
        private readonly IAppDbContext _db;
        private readonly IOptions<MediaOptions> _mediaOpt;

        private static readonly string[] _cultures = new[] { "ka", "en" };

        public GetDonorDetailsHandler(
            IAppDbContext db,
            IOptions<MediaOptions> mediaOpt)
        {
            _db = db;
            _mediaOpt = mediaOpt;
        }

        public async Task<DonorAdminDto> Handle(GetDonorDetailsQuery q, CancellationToken ct)
        {
            if (q.Id <= 0)
            {
                return new DonorAdminDto
                {
                    IsActive = true,
                    LogoFilePath = null,
                    Locales = _cultures
                        .Select(c => new DonorLocaleAdminDto
                        {
                            Culture = c,
                            Name = string.Empty,
                            Description = null
                        })
                        .ToList()
                };
            }

            var donor = await _db.Donors
                .AsNoTracking()
                .Include(d => d.Locales)
                .FirstOrDefaultAsync(d => d.Id == q.Id, ct);

            if (donor is null)
                throw new NotFoundException($"Donor {q.Id} not found");

            var logoAsset = await _db.MediaAttachments
                .AsNoTracking()
                .Where(a =>
                    a.OwnerType == MediaOwnerType.Donors &&
                    a.OwnerKey == donor.Id.ToString())
                .OrderByDescending(a => a.IsCover)
                .Select(a => a.MediaAsset)
                .FirstOrDefaultAsync(ct);

            var localesDict = donor.Locales.ToDictionary(x => x.Culture, x => x);

            return new DonorAdminDto
            {
                Id = donor.Id,
                IsActive = donor.IsActive,
                WebsiteUrl = donor.WebsiteUrl,

                LogoFilePath = MediaUrlHelper.ToUrl(_mediaOpt.Value, logoAsset?.StoredPath),

                Locales = _cultures
                    .Select(c =>
                    {
                        if (localesDict.TryGetValue(c, out var loc))
                        {
                            return new DonorLocaleAdminDto
                            {
                                Id = loc.Id,
                                Culture = loc.Culture,
                                Name = loc.Name,
                                Description = loc.Description
                            };
                        }

                        return new DonorLocaleAdminDto
                        {
                            Culture = c
                        };
                    }).ToList()
            };
        }
    }
}
