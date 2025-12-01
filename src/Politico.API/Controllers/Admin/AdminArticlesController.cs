using MediatR;
using Microsoft.AspNetCore.Mvc;
using Politico.API.Settings;
using Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Create;
using Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Delete;
using Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Remove;
using Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Update;
using Politico.Application.Handlers.Admin.ArticeleModuls.Queries.GetArticleDetails;
using Politico.Application.Handlers.Admin.ArticeleModuls.Queries.GetArticleList;
using Politico.Domain.Common.Enums.Articles;

namespace Politico.API.Controllers.Admin;

[Route("api/v{version:apiVersion}/{culture}/admin/articles")]
public sealed class AdminArticlesController : ApiControllerBase
{
    public AdminArticlesController(ISender sender) : base(sender) { }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleCommand cmd)
    {
        var id = await Sender.Send(cmd);
        return Ok(id);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateArticleCommand cmd)
    {
        cmd.Id = id;
        await Sender.Send(cmd);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await Sender.Send(new DeleteArticleCommand(id));
        return NoContent();
    }

    [HttpDelete("{id:long}/hard")]
    public async Task<IActionResult> HardDelete(long id)
    {
        await Sender.Send(new RemoveArticleCommand(id));
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] ArticleCategory? category,
        [FromQuery] ArticleStatus? status,
        [FromQuery] bool? isActive)
    {
        var items = await Sender.Send(new GetArticleAdminListQuery
        {
            Category = category,
            Status = status,
            IsActive = isActive
        });

        return Ok(items);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Details(long id)
    {
        var dto = await Sender.Send(new GetArticleAdminDetailsQuery(id));
        return Ok(dto);
    }
}
