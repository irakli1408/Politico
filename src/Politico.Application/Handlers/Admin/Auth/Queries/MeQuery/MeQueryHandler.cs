using MediatR;
using Politico.Application.DTO.Auth;
using Politico.Application.Interfaces.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Queries.MeQuery
{
    public sealed class MeQueryHandler : IRequestHandler<MeQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public MeQueryHandler(
            IUserRepository userRepository,
            ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<UserDto> Handle(
            MeQuery request,
            CancellationToken cancellationToken)
        {
            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var user = await _userRepository.GetByIdAsync(_currentUser.UserId.Value, cancellationToken);
            if (user is null)
                throw new UnauthorizedAccessException("User not found.");

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToArray();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive,
                Roles = roles
            };
        }
    }
}
