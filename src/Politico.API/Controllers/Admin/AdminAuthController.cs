using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Politico.API.Settings;
using Politico.Application.DTO.Auth;
using Politico.Application.Handlers.Admin.Auth.Command.Login;
using Politico.Application.Handlers.Admin.Auth.Command.Logout;
using Politico.Application.Handlers.Admin.Auth.Command.MakeAdmin;
using Politico.Application.Handlers.Admin.Auth.Command.RefreshToken;
using Politico.Application.Handlers.Admin.Auth.Command.Register;
using Politico.Application.Handlers.Admin.Auth.Command.RemoveAdmin;
using Politico.Application.Handlers.Admin.Auth.Queries.MeQuery;

namespace Politico.API.Controllers.Admin
{
    [ApiController]
    [Route("api/v{version:apiVersion}/{culture}/[controller]")]
    public class AdminAuthController : ApiControllerBase
    {
        public AdminAuthController(ISender sender) : base(sender) { }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResultDto>> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var result = await Sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResultDto>> Refresh(
            [FromBody] RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var result = await Sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize] // достаточно просто авторизованного пользователя
        public async Task<IActionResult> Logout(
            [FromBody] LogoutCommand command,
            CancellationToken cancellationToken)
        {
            await Sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("me")]
        [Authorize] // любой авторизованный
        public async Task<ActionResult<UserDto>> Me(CancellationToken cancellationToken)
        {
            var result = await Sender.Send(new MeQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost("register-admin")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<ActionResult<UserDto>> RegisterAdmin(
            [FromBody] RegisterCommand command,   // при желании сделай отдельный RegisterAdminCommand
            CancellationToken cancellationToken)
        {
            // например, в команде может быть флаг IsAdmin = true
            var result = await Sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("make-admin")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> MakeAdmin(
            [FromBody] MakeAdminCommand command,
            CancellationToken cancellationToken)
        {
            await Sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("remove-admin")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> RemoveAdmin(
        [FromBody] RemoveAdminCommand command,
        CancellationToken cancellationToken)
        {
            await Sender.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
