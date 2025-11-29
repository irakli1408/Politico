using MediatR;
using Politico.Application.DTO.Auth;
using Politico.Application.Interfaces.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.RefreshToken
{
    public sealed class RefreshTokenCommandHandler
         : IRequestHandler<RefreshTokenCommand, AuthResultDto>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUser;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            ICurrentUserService currentUser)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _currentUser = currentUser;
        }

        public async Task<AuthResultDto> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var existingToken = await _refreshTokenRepository
                .GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (existingToken is null || !existingToken.IsActive)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            var user = existingToken.User;

            if (!user.IsActive)
                throw new UnauthorizedAccessException("User is blocked.");

            var roles = user.UserRoles.Select(ur => ur.Role).ToArray();

            // Генерируем новую пару токенов
            var (accessToken, expiresAtUtc) = _tokenService.GenerateAccessToken(user, roles);

            var ip = _currentUser.IpAddress ?? "unknown";
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id, ip);

            // Отзываем старый refresh token и указываем, какой его заменил
            existingToken.Revoke(ip, newRefreshToken.Token);

            user.AddRefreshToken(newRefreshToken);
            await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
            await _refreshTokenRepository.UpdateAsync(existingToken, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return new AuthResultDto
            {
                AccessToken = accessToken,
                AccessTokenExpiresAtUtc = expiresAtUtc,
                RefreshToken = newRefreshToken.Token,
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.Select(r => r.Name).ToArray()
            };
        }
    }
}
