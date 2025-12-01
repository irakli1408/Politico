using MediatR;
using Microsoft.AspNetCore.Mvc;
using Politico.API.Settings;
using Politico.Application.DTO.AboutOrg.Team;
using Politico.Application.Handlers.Admin.Team.Commands.HardDelete;
using Politico.Application.Handlers.Admin.Team.Commands.Upsert;
using Politico.Application.Handlers.Admin.Team.Queries.GetDeteils;
using Politico.Application.Handlers.Admin.Team.Queries.GetList;

namespace Politico.API.Controllers.Admin
{
    [Route("api/v{version:apiVersion}/{culture}/admin/team")]
    public class AdminTeamController : ApiControllerBase
    {
        public AdminTeamController(ISender sender) : base(sender) { }

        [HttpGet]
        public async Task<ActionResult<List<TeamMemberListItemAdminDto>>> GetList(
              [FromQuery] bool onlyActive = false,
              CancellationToken ct = default)
        {
            var result = await Sender.Send(new GetTeamMemberAdminListQuery(onlyActive), ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TeamMemberAdminDetailsDto>> GetDetails(
            int id,
            CancellationToken ct = default)
        {
            var result = await Sender.Send(new GetTeamMemberAdminDetailsQuery(id), ct);
            return Ok(result);
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<int>> Upsert(
        [FromBody] TeamMemberUpsertDto dto,
        CancellationToken ct = default)
        {
            // Id = 0 → create, Id > 0 → update
            var id = await Sender.Send(new UpsertTeamMemberAdminCommand(dto), ct);
            return Ok(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await Sender.Send(new DeleteTeamMemberAdminCommand(id), ct);
            return NoContent();
        }

    }
}
