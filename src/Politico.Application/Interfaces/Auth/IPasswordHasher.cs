namespace Politico.Application.Interfaces.Auth
{
    /// <summary>
    /// Абстракция над хешированием пароля.
    /// В Infrastructure можно будет использовать ASP.NET Core Identity PasswordHasher или свой алгоритм.
    /// </summary>
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}
