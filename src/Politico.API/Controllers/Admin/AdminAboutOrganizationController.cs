using MediatR;
using Microsoft.AspNetCore.Mvc;
using Politico.API.Settings;
using Politico.Application.DTO.AboutOrg;
using Politico.Application.Features.AboutOrganization.Queries.GetAboutAdmin;
using Politico.Application.Handlers.Admin.AboutOrg.Commands.Upsert;

namespace Politico.API.Controllers.Admin
{
    [Route("api/v{version:apiVersion}/{culture}/admin/about")]
    public class AdminAboutOrganizationController : ApiControllerBase
    {
        public AdminAboutOrganizationController(ISender sender) : base(sender) { }

        [HttpGet]
        public async Task<ActionResult<AboutOrganizationAdminDto>> Get(CancellationToken ct)
        {
            var result = await Sender.Send(new GetAboutOrganizationQuery(), ct);
            return Ok(result);
        }
       
        [HttpPut]
        public async Task<IActionResult> Upsert(
            [FromBody] AboutOrganizationDto model,
            CancellationToken ct)
        {
            await Sender.Send(new UpsertAboutOrganizationACommand(model), ct);
            return NoContent();
        }
    }
}
