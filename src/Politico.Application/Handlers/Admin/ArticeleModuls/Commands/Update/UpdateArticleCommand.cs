using MediatR;
using Politico.Application.DTO.ArticeleModuls;
using Politico.Domain.Common.Enums.Articles;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Update
{
    public sealed class UpdateArticleCommand : IRequest<Unit>
    {
        public long Id { get; set; }

        public ArticleCategory Category { get; set; }
        public ArticleStatus Status { get; set; }
        public bool IsActive { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsFeatured { get; set; }
        public int PriorityScore { get; set; }

        public List<ArticleLocaleDto> Locales { get; set; } = new();
    }
}
