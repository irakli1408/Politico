namespace Politico.Application.DTO.Auth
{
    public sealed class UserDto
    {
        public long Id { get; init; }
        public string Email { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public bool IsActive { get; init; }
        public string[] Roles { get; init; } = [];
    }
}
