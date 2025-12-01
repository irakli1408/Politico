using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.AboutOrg;

namespace Politico.Application.Handlers.Admin.Team.Commands.Upsert
{
    public sealed class UpsertTeamMemberAdminHandler
        : IRequestHandler<UpsertTeamMemberAdminCommand, int>
    {
        private readonly IAppDbContext _db;

        public UpsertTeamMemberAdminHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<int> Handle(
            UpsertTeamMemberAdminCommand request,
            CancellationToken ct)
        {
            var dto = request.Model;

            TeamMember entity;

            if (dto.Id > 0)
            {
                entity = await _db.TeamMembers
                    .Include(t => t.Locales)
                    .FirstOrDefaultAsync(t => t.Id == dto.Id, ct)
                    ?? throw new Exception($"TeamMember {dto.Id} not found");
            }
            else
            {
                entity = new TeamMember();
                _db.TeamMembers.Add(entity);
            }

            entity.IsActive = dto.IsActive;
            entity.OrderIndex = dto.OrderIndex;

            foreach (var localeDto in dto.Locales)
            {
                var loc = entity.Locales
                    .FirstOrDefault(l => l.Culture == localeDto.Culture);

                if (loc is null)
                {
                    loc = new TeamMemberLocale
                    {
                        Culture = localeDto.Culture,
                        Name = localeDto.Name,
                        Position = localeDto.Position,
                        Bio = localeDto.Bio
                    };
                    entity.Locales.Add(loc);
                }
                else
                {
                    loc.Name = localeDto.Name;
                    loc.Position = localeDto.Position;
                    loc.Bio = localeDto.Bio;
                }
            }

            await _db.SaveChangesAsync(ct);
            return entity.Id;
        }
    }
}
