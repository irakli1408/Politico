using MediatR;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Remove
{
    public sealed record RemoveArticleCommand(long Id) : IRequest<Unit>;
}
