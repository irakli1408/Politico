using Politico.Domain.Entities.Auth;

namespace Politico.Application.Interfaces.Auth
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Role role, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
