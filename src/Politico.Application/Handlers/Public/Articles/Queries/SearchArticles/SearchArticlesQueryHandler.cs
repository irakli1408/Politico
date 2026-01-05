using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Common.Paging;
using Politico.Application.Handlers.Public.Articles.Queries.SearchArticles;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.CurrentState;

namespace Politico.Application.Features.Articles.Queries.SearchArticles;

public sealed class SearchArticlesQueryHandler
    : IRequestHandler<SearchArticlesQuery, PagedResult<ArticleSearchItemVm>>
{
    private readonly IAppDbContext _db;
    private readonly ICurrentStateService _currentState;

    public SearchArticlesQueryHandler(
        IAppDbContext db,
        ICurrentStateService currentState)
    {
        _db = db;
        _currentState = currentState;
    }

    public async Task<PagedResult<ArticleSearchItemVm>> Handle(
        SearchArticlesQuery request,
        CancellationToken ct)
    {
        var culture = (request.Culture ?? "en").Trim().ToLowerInvariant();
        culture = culture.Split('-')[0];
        var query = request.Query.Trim();

        if (query.Length < 2)
            return PagedResult<ArticleSearchItemVm>.Empty(
                request.Skip,
                request.Take);

        var baseQuery = _db.ArticleLocales
            .AsNoTracking()
            .Where(x => x.DeleteDate == null)
            .Where(x => x.Culture == culture)
            .Where(x =>
                x.ShortSummary != null &&
                EF.Functions.Like(x.ShortSummary, $"%{query}%"));

        var total = await baseQuery.CountAsync(ct);

        var items = await baseQuery
            .OrderByDescending(x => x.Id)
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(x => new ArticleSearchItemVm(
                x.ArticleId,
                x.Culture,
                x.Title,
                x.ShortSummary!,
                x.Slug
            ))
            .ToListAsync(ct);

        return new PagedResult<ArticleSearchItemVm>(
            items,
            request.Skip,
            request.Take,
            total);
    }
}
