using MediatR;
using Politico.Application.DTO.Auth;
using Politico.Application.Interfaces.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.Login
{
    public sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICurrentUserService _currentUser;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository,
            ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _currentUser = currentUser;
        }

        public async Task<AuthResultDto> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Находим пользователя по email
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // 2. Проверяем блокировку
            if (!user.IsActive)
                throw new UnauthorizedAccessException("User is blocked.");

            // 3. Проверяем пароль
            var passwordValid = _passwordHasher.VerifyHashedPassword(
                user.PasswordHash,
                request.Password);

            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // 4. Достаём роли
            var roles = user.UserRoles.Select(ur => ur.Role).ToArray();

            // 5. Генерируем access token
            var (accessToken, expiresAtUtc) = _tokenService.GenerateAccessToken(user, roles);

            // 6. Генерируем refresh token
            var ip = _currentUser.IpAddress ?? "unknown";
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id, ip);

            // 7. Привязываем refresh token к пользователю и сохраняем
            user.AddRefreshToken(refreshToken);
            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            // Достаточно одного SaveChanges, т.к. контекст общий
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            // 8. Возвращаем DTO
            return new AuthResultDto
            {
                AccessToken = accessToken,
                AccessTokenExpiresAtUtc = expiresAtUtc,
                RefreshToken = refreshToken.Token,
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.Select(r => r.Name).ToArray()
            };
        }
    }
}
