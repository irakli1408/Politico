using Microsoft.AspNetCore.Identity;
using Politico.Application.Interfaces.Auth;      // твой интерфейс
using Politico.Domain.Entities.Auth;            // твой User

namespace Politico.Persistence.Identity
{
    public sealed class PasswordHasherAdapter : IPasswordHasher
    {
        private readonly PasswordHasher<User> _inner = new();

        public string HashPassword(string password)
        {
            return _inner.HashPassword(user: null!, password);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var result = _inner.VerifyHashedPassword(user: null!, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
