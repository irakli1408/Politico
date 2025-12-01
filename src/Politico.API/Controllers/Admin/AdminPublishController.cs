using MediatR;
using Microsoft.AspNetCore.Mvc;
using Politico.API.Settings;
using Politico.Domain.Common.Enums.Media;
using Politico.Domain.Entities.AboutOrg;
using Politico.Domain.Entities.Articels;
using Politico.Domain.Entities.Donors;

namespace Politico.API.Controllers.Admin
{
    [Route("api/v{version:apiVersion}/{culture}/admin/publish")]
    public class AdminPublishController : ApiControllerBase
    {
        public AdminPublishController(ISender sender) : base(sender) { }

        [HttpPost("{ownerType}/{id:long}")]
        public async Task<IActionResult> SetActive(MediaOwnerType ownerType, long id, [FromQuery] bool active)
        {
            switch (ownerType)
            {
                case MediaOwnerType.Article:
                    await Sender.Send(new SetActiveCommand<Article, long>(id, active));
                    break;

                case MediaOwnerType.AboutOrganization:
                    // TODO
                    break;


                case MediaOwnerType.TeamMember:
                    await Sender.Send(new SetActiveCommand<TeamMember, int>((int)id, active));
                    break;

                case MediaOwnerType.Donors:
                    await Sender.Send(new SetActiveCommand<Donor, int>((int)id, active));
                    break;

                default:
                    return BadRequest("Invalid owner type.");
            }

            return NoContent();
        }
    }
}
