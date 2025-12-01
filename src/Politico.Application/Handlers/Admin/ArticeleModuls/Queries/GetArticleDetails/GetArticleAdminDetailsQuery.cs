using MediatR;
using Politico.Application.DTO.ArticeleModuls;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Queries.GetArticleDetails
{
    public sealed record GetArticleAdminDetailsQuery(long Id) : IRequest<ArticleAdminDetailsDto>;

}
