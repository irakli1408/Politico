using Politico.Domain.Common.Enums.Articles;

namespace Politico.Application.DTO.ArticeleModuls
{
    public sealed class ArticleAdminListItemDto
    {
        public long Id { get; set; }
        public ArticleCategory Category { get; set; }
        public ArticleStatus Status { get; set; }
        public bool IsActive { get; set; }
        public DateTime PublishDate { get; set; }

        // удобное поле – заголовок на главном языке
        public string? Title { get; set; }
    }
}
