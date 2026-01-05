using MediatR;

namespace Politico.Application.Handlers.Public.Articles.Queries.GetArticleBySlug;

public sealed record GetArticleBySlugQuery(
    string Culture,
    string Slug
) : IRequest<ArticleDetailsVm?>;
