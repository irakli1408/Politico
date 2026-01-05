namespace Politico.Application.Handlers.Public.Articles.Queries.SearchArticles;

public sealed record ArticleSearchItemVm(
    long ArticleId,
    string Culture,
    string Title,
    string ShortSummary,
    string Slug
);
