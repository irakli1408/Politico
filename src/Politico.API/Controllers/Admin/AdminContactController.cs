using MediatR;
using Microsoft.AspNetCore.Mvc;
using Politico.API.Settings;
using Politico.Application.DTO.Contact;
using Politico.Application.Handlers.Admin.Contact.Commands.Upsert;
using Politico.Application.Handlers.Admin.Contact.Queries;

namespace Politico.API.Controllers.Admin
{
    [Route("api/v{version:apiVersion}/{culture}/admin/contact")]
    public class AdminContactController : ApiControllerBase
    {
        public AdminContactController(ISender sender) : base(sender) { }

        [HttpGet]
        public async Task<ActionResult<ContactAdminDto>> Get(CancellationToken ct)
        {
            var result = await Sender.Send(new GetContactAdminQuery(), ct);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Upsert(
            [FromBody] ContactAdminDto model,
            CancellationToken ct)
        {
            await Sender.Send(new UpsertContactAdminCommand(model), ct);
            return NoContent();
        }
    }
}
