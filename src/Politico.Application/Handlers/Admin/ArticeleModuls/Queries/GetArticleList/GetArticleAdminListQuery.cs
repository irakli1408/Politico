using MediatR;
using Politico.Application.DTO.ArticeleModuls;
using Politico.Domain.Common.Enums.Articles;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Queries.GetArticleList
{
    public sealed class GetArticleAdminListQuery : IRequest<List<ArticleAdminListItemDto>>
    {
        public ArticleCategory? Category { get; set; }
        public ArticleStatus? Status { get; set; }
        public bool? IsActive { get; set; }
    }
}
