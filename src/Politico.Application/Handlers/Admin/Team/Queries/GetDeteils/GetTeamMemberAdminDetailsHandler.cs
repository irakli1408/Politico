using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Politico.Application.Common.Helper.Model;
using Politico.Application.DTO.AboutOrg.Team;
using Politico.Application.DTO.ArticeleModuls;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Exceptions;
using Politico.Domain.Common.Enums.Media;
using Politico.Domain.Entities.AboutOrg;

namespace Politico.Application.Handlers.Admin.Team.Queries.GetDeteils
{
    public sealed class GetTeamMemberAdminDetailsHandler
       : IRequestHandler<GetTeamMemberAdminDetailsQuery, TeamMemberAdminDetailsDto>
    {
        private readonly IAppDbContext _db;
        private readonly MediaOptions _mediaOpt;

        private static readonly string[] _cultures = new[] { "ka", "en" };

        public GetTeamMemberAdminDetailsHandler(
            IAppDbContext db,
            IOptions<MediaOptions> mediaOpt)
        {
            _db = db;
            _mediaOpt = mediaOpt.Value;
        }

        public async Task<TeamMemberAdminDetailsDto> Handle(
            GetTeamMemberAdminDetailsQuery request,
            CancellationToken ct)
        {
            var member = await _db.TeamMembers
                .Include(t => t.Locales)
                .FirstOrDefaultAsync(t => t.Id == request.Id, ct);

            if (member is null)
                throw new NotFoundException(nameof(TeamMember), request.Id);

            var attachments = await _db.MediaAttachments
                .Include(m => m.MediaAsset)
                .Where(m =>
                    m.OwnerType == MediaOwnerType.TeamMember &&
                    m.OwnerKey == member.Id.ToString())
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

            var cover = mediaDtos.FirstOrDefault(x => x.IsCover)?.AssetId;

            return new TeamMemberAdminDetailsDto
            {
                Id = member.Id,
                IsActive = member.IsActive,
                OrderIndex = member.OrderIndex,

                Locales = _cultures
                    .Select(culture =>
                    {
                        var loc = member.Locales.FirstOrDefault(l => l.Culture == culture);
                        return new TeamMemberLocaleAdminDto
                        {
                            Culture = culture,
                            Name = loc?.Name ?? string.Empty,
                            Position = loc?.Position ?? string.Empty,
                            Bio = loc?.Bio ?? string.Empty
                        };
                    })
                    .ToList(),

                CoverId = cover,
                Media = mediaDtos
            };
        }
    }
}
