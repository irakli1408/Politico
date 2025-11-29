using MediatR;
using Politico.Application.DTO.Auth;
using Politico.Application.Interfaces.Auth;
using Politico.Domain.Entities.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.Register
{
    public sealed class RegisterHandler : IRequestHandler<RegisterCommand, UserDto>
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly IPasswordHasher _hasher;

        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken ct)
        {
            if (await _users.EmailExistsAsync(request.Email, ct))
                throw new InvalidOperationException("Email already exists.");

            var hash = _hasher.HashPassword(request.Password);
            var user = new User(request.Email, request.UserName, hash);

            var roleName = request.IsAdmin ? RoleNames.Admin : RoleNames.User;
            var role = await _roles.GetByNameAsync(roleName, ct)
                       ?? throw new InvalidOperationException("Role not found.");

            user.AssignRole(role);

            await _users.AddAsync(user, ct);
            await _users.SaveChangesAsync(ct);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive,
                Roles = new[] { role.Name }
            };
        }
    }
}