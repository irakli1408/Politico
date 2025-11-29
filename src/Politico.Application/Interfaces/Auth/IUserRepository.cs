using Politico.Domain.Entities.Auth;

namespace Politico.Application.Interfaces.Auth
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохранить изменения (если репозиторий сам контролирует UnitOfWork).
        /// Либо можно использовать отдельный IUnitOfWork, если он у тебя есть.
        /// </summary>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
