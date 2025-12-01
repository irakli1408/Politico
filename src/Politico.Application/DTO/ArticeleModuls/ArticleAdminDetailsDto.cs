using Politico.Domain.Common.Enums.Articles;

namespace Politico.Application.DTO.ArticeleModuls
{
    public sealed class ArticleAdminDetailsDto
    {
        public long Id { get; set; }

        public ArticleCategory Category { get; set; }
        public ArticleStatus Status { get; set; }
        public bool IsActive { get; set; }
        public DateTime PublishDate { get; set; }

        public bool IsFeatured { get; set; }
        public int PriorityScore { get; set; }

        public List<ArticleLocaleDto> Locales { get; set; } = new(); 
        public CommonMediaItemDto? Cover { get; set; }
        public List<CommonMediaItemDto> Media { get; set; } = new();
    }
}
