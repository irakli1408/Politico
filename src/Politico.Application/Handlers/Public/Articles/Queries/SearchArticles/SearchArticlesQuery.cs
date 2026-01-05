using MediatR;
using Politico.Application.Common.Paging;
using Politico.Application.Handlers.Public.Articles.Queries.SearchArticles;

namespace Politico.Application.Features.Articles.Queries.SearchArticles;

public sealed record SearchArticlesQuery(
    string Culture,
    string Query,
    int Skip = 0,
    int Take = 20
) : IRequest<PagedResult<ArticleSearchItemVm>>;