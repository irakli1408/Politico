using MediatR;
using Politico.Application.Interfaces.Auth;
using Politico.Domain.Entities.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.RemoveAdmin
{
    public sealed class RemoveAdminHandler : IRequestHandler<RemoveAdminCommand, Unit>
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly ICurrentUserService _currentUser;

        public RemoveAdminHandler(
            IUserRepository users,
            IRoleRepository roles,
            ICurrentUserService currentUser)
        {
            _users = users;
            _roles = roles;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(RemoveAdminCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Проверяем, что действие делает SUPERADMIN
            if (!_currentUser.Roles.Contains(RoleNames.SuperAdmin))
                throw new UnauthorizedAccessException("Only SuperAdmin can remove admin role.");

            // 2️⃣ Находим пользователя
            var user = await _users.GetByIdAsync(request.UserId, cancellationToken)
                       ?? throw new InvalidOperationException("User not found.");

            // 3️⃣ Находим роль Admin
            var adminRole = await _roles.GetByNameAsync(RoleNames.Admin, cancellationToken)
                            ?? throw new InvalidOperationException("Role 'Admin' not found.");

            // 4️⃣ Проверяем, что у пользователя есть эта роль
            if (!user.UserRoles.Any(ur => ur.RoleId == adminRole.Id))
                throw new InvalidOperationException("User is not an Admin.");

            // 5️⃣ Запрещаем SuperAdmin снимать роль у самого себя (опционально, но безопасно)
            if (_currentUser.UserId == user.Id)
                throw new InvalidOperationException("SuperAdmin cannot remove own admin role.");

            // 6️⃣ Удаляем роль
            user.RemoveRole(adminRole);

            await _users.UpdateAsync(user, cancellationToken);
            await _users.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
