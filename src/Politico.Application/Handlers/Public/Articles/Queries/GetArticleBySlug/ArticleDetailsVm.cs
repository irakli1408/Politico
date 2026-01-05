namespace Politico.Application.Handlers.Public.Articles.Queries.GetArticleBySlug;

public sealed record ArticleDetailsVm(
    long ArticleId,
    string Culture,
    string Title,
    string ShortSummary,
    string Content,
    string Slug
);
