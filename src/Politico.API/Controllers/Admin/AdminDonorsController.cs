using MediatR;
using Microsoft.AspNetCore.Mvc;
using Politico.API.Settings;
using Politico.Application.DTO.Donors;
using Politico.Application.Handlers.Admin.Donors.Commands.Create;
using Politico.Application.Handlers.Admin.Donors.Commands.Delete;
using Politico.Application.Handlers.Admin.Donors.Commands.Update;
using Politico.Application.Handlers.Admin.Donors.Queries.GetDeteils;
using Politico.Application.Handlers.Admin.Donors.Queries.GetList;

namespace Politico.API.Controllers.Admin
{
    [Route("api/v{version:apiVersion}/{culture}/admin/donors")]
    public sealed class AdminDonorsController : ApiControllerBase
    {
        public AdminDonorsController(ISender sender) : base(sender) { }

        [HttpGet]
        public async Task<ActionResult<List<DonorListItemDto>>> GetList(
            string culture,
            [FromQuery] bool? onlyActive,
            CancellationToken ct)
        {
            var result = await Sender.Send(new GetDonorListQuery(onlyActive), ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DonorAdminDto>> GetDetails(
            string culture,
            int id,
            CancellationToken ct)
        {
            var dto = await Sender.Send(new GetDonorDetailsQuery(id), ct);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(
       string culture,
       [FromBody] DonorAdminDto dto,
       CancellationToken ct)
        {
            var id = await Sender.Send(new CreateDonorCommand
            {
                Donor = dto
            }, ct);

            return Ok(new { id });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(
            string culture,
            int id,
            [FromBody] DonorAdminDto dto,
            CancellationToken ct)
        {
            dto.Id = id;

            await Sender.Send(new UpdateDonorCommand
            {
                Donor = dto
            }, ct);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(
            string culture,
            int id,
            CancellationToken ct)
        {
            await Sender.Send(new DeleteDonorCommand(id), ct);
            return NoContent();
        }
    }
}
