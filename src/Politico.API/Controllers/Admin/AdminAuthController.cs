using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/admin/auth")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminAuthController(IMediator mediator) { _mediator = mediator; }

        // ---------- LOGIN ----------

        /// <summary>Логин админа / супер-админа. Публичный endpoint.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResultDto>> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // ---------- REFRESH ----------

        /// <summary>Обновление access/refresh токенов по действующему refresh токену.</summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResultDto>> Refresh(
            [FromBody] RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // ---------- LOGOUT ----------

        /// <summary>Logout — ревокация refresh токена.</summary>
        [HttpPost("logout")]
        [Authorize] // достаточно просто авторизованного пользователя
        public async Task<IActionResult> Logout(
            [FromBody] LogoutCommand command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // ---------- ME ----------

        /// <summary>Информация о текущем авторизованном пользователе (админ/суперадмин).</summary>
        [HttpGet("me")]
        [Authorize] // любой авторизованный
        public async Task<ActionResult<UserDto>> Me(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new MeQuery(), cancellationToken);
            return Ok(result);
        }

        // ---------- REGISTER ADMIN ----------

        /// <summary>Создать нового администратора. Только для SuperAdmin.</summary>
        [HttpPost("register-admin")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<ActionResult<UserDto>> RegisterAdmin(
            [FromBody] RegisterCommand command,   // при желании сделай отдельный RegisterAdminCommand
            CancellationToken cancellationToken)
        {
            // например, в команде может быть флаг IsAdmin = true
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // ---------- MAKE ADMIN ----------

        /// <summary>Назначить пользователю роль Admin. Только SuperAdmin.</summary>
        [HttpPost("make-admin")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> MakeAdmin(
            [FromBody] MakeAdminCommand command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // ---------- REMOVE ADMIN ----------

        /// <summary>Снять с пользователя роль Admin. Только SuperAdmin.</summary>
        [HttpPost("remove-admin")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> RemoveAdmin(
        [FromBody] RemoveAdminCommand command,
        CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
