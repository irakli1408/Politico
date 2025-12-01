using MediatR;
using Politico.Application.DTO.ArticeleModuls;
using Politico.Domain.Common.Enums.Articles;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Create
{
    public sealed class CreateArticleCommand : IRequest<long>
    {
        public ArticleCategory Category { get; set; }
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
        public bool IsActive { get; set; } = false;

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public bool IsFeatured { get; set; }
        public int PriorityScore { get; set; }

        public List<ArticleLocaleDto> Locales { get; set; } = new();
    }
}
