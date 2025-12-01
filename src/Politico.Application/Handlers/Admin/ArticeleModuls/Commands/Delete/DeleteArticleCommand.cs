using MediatR;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Delete
{
    public sealed record DeleteArticleCommand(long Id) : IRequest<Unit>;
}
