using MediatR;
using Politico.Application.Interfaces.Auth;

namespace Politico.Application.Handlers.Admin.Auth.Command.Logout
{
    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICurrentUserService _currentUser;

        public LogoutCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            ICurrentUserService currentUser)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenRepository
                .GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (token is null || !token.IsActive)
                return Unit.Value; // уже нет токена — считаем успехом

            var ip = _currentUser.IpAddress ?? "unknown";
            token.Revoke(ip);

            await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
