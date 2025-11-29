using MediatR;
using Politico.Application.Interfaces.Auth;
using Politico.Domain.Entities.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.MakeAdmin
{
    public sealed class MakeAdminHandler : IRequestHandler<MakeAdminCommand, Unit>
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly ICurrentUserService _current;

        public async Task<Unit> Handle(MakeAdminCommand request, CancellationToken ct)
        {
            // 1. Только SuperAdmin
            if (!_current.Roles.Contains(RoleNames.SuperAdmin))
                throw new UnauthorizedAccessException("Only SuperAdmin can assign admins.");

            var user = await _users.GetByIdAsync(request.UserId, ct)
                       ?? throw new InvalidOperationException("User not found.");

            var adminRole = await _roles.GetByNameAsync(RoleNames.Admin, ct)
                             ?? throw new InvalidOperationException("Role Admin not found.");

            user.AssignRole(adminRole);

            await _users.UpdateAsync(user, ct);
            await _users.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}