using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Politico.Application.Common.Helper.Model;
using Politico.Application.DTO.AboutOrg.Team;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.CurrentState;
using Politico.Domain.Common.Enums.Media;

namespace Politico.Application.Handlers.Admin.Team.Queries.GetList
{
    public sealed class GetTeamMemberAdminListHandler
       : IRequestHandler<GetTeamMemberAdminListQuery, List<TeamMemberListItemAdminDto>>
    {
        private readonly IAppDbContext _db;
        private readonly MediaOptions _mediaOpt;
        private readonly ICurrentStateService _state;

        public GetTeamMemberAdminListHandler(
            IAppDbContext db,
            IOptions<MediaOptions> mediaOpt,
            ICurrentStateService state)
        {
            _db = db;
            _mediaOpt = mediaOpt.Value;
            _state = state;
        }

        public async Task<List<TeamMemberListItemAdminDto>> Handle(
            GetTeamMemberAdminListQuery request,
            CancellationToken ct)
        {
            var culture = _state.AcceptedLanguage; // "ka" / "en" и т.д.

            var query = _db.TeamMembers
                .Include(t => t.Locales)
                // если хочешь учитывать soft delete:
                //.Where(t => !t.IsDeleted)
                .AsQueryable();

            if (request.OnlyActive)
            {
                query = query.Where(t => t.IsActive);
            }

            var members = await query
                .OrderBy(t => t.OrderIndex)
                .ThenBy(t => t.Id)
                .ToListAsync(ct);

            var ids = members.Select(m => m.Id.ToString()).ToList();

            var attachments = await _db.MediaAttachments
                .Include(m => m.MediaAsset)
                .Where(m =>
                    m.OwnerType == MediaOwnerType.TeamMember &&
                    ids.Contains(m.OwnerKey) &&
                    m.IsCover)
                .ToListAsync(ct);

            var result = new List<TeamMemberListItemAdminDto>();

            foreach (var member in members)
            {
                var locale = member.Locales
                    .FirstOrDefault(l => l.Culture == culture)
                    ?? member.Locales.FirstOrDefault();

                var coverAttach = attachments
                    .FirstOrDefault(a => a.OwnerKey == member.Id.ToString());

                string? photoUrl = null;

                if (coverAttach?.MediaAsset?.ThumbStoredPath != null)
                {
                    photoUrl = MediaUrlHelper.ToUrl(_mediaOpt, coverAttach.MediaAsset.ThumbStoredPath);
                }
                else if (coverAttach?.MediaAsset?.StoredPath != null)
                {
                    photoUrl = MediaUrlHelper.ToUrl(_mediaOpt, coverAttach.MediaAsset.StoredPath);
                }

                result.Add(new TeamMemberListItemAdminDto
                {
                    Id = member.Id,
                    IsActive = member.IsActive,
                    OrderIndex = member.OrderIndex,
                    Name = locale?.Name ?? string.Empty,
                    Position = locale?.Position ?? string.Empty,
                    PhotoUrl = photoUrl
                });
            }

            return result;
        }
    }
}
