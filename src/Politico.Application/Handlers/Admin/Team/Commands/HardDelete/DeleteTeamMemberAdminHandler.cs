using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Exceptions;
using Politico.Domain.Common.Enums.Media;
using Politico.Domain.Entities.AboutOrg;

namespace Politico.Application.Handlers.Admin.Team.Commands.HardDelete
{
    public sealed class DeleteTeamMemberAdminHandler
        : IRequestHandler<DeleteTeamMemberAdminCommand, Unit>
    {
        private readonly IAppDbContext _db;

        public DeleteTeamMemberAdminHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(
            DeleteTeamMemberAdminCommand request,
            CancellationToken ct)
        {
            // 1) Находим TeamMember
            var member = await _db.TeamMembers
                .FirstOrDefaultAsync(t => t.Id == request.Id, ct);

            if (member is null)
                throw new NotFoundException(nameof(TeamMember), request.Id);

            // 2) Удаляем привязки медиа (MediaAttachments) для этого TeamMember
            var attachments = await _db.MediaAttachments
                .Where(m =>
                    m.OwnerType == MediaOwnerType.TeamMember &&
                    m.OwnerKey == request.Id.ToString())
                .ToListAsync(ct);

            _db.MediaAttachments.RemoveRange(attachments);

            // 3) Жёстко удаляем самого TeamMember
            _db.TeamMembers.Remove(member);

            await _db.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
