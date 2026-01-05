using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;

namespace Politico.Application.Handlers.Public.Articles.Queries.GetArticleBySlug;

public sealed class GetArticleBySlugQueryHandler
    : IRequestHandler<GetArticleBySlugQuery, ArticleDetailsVm?>
{
    private readonly IAppDbContext _db;

    public GetArticleBySlugQueryHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<ArticleDetailsVm?> Handle(
        GetArticleBySlugQuery request,
        CancellationToken ct)
    {
        var culture = (request.Culture ?? "en").Trim().ToLowerInvariant();
        culture = culture.Split('-')[0];

        var slug = (request.Slug ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(slug))
            return null;

        return await _db.ArticleLocales
            .AsNoTracking()
            .Where(x => x.DeleteDate == null)
            .Where(x => x.Culture == culture)
            .Where(x => x.Slug == slug)
            .Select(x => new ArticleDetailsVm(
                x.ArticleId,
                x.Culture,
                x.Title,
                x.ShortSummary ?? string.Empty,
                x.Content ?? string.Empty,
                x.Slug
            ))
            .FirstOrDefaultAsync(ct);
    }
}
