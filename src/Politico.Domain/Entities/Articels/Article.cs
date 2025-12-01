using Politico.Domain.Common.Enums.Articles;
using Politico.Domain.Common.Interfaces;

namespace Politico.Domain.Entities.Articels
{
    public class Article : IHasId<long>, IActivatable, IEntity, IHasDeleteDate
    {
        public long Id { get; set; }

        public ArticleCategory Category { get; set; }

        public ArticleStatus Status { get; set; }      // Draft / Ready
        public bool IsActive { get; set; }             // опубликовано на сайте

        public DateTime PublishDate { get; set; }

        public bool IsFeatured { get; set; }
        public int PriorityScore { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public ICollection<ArticleLocale> Locales { get; set; } = new List<ArticleLocale>();
    }
}
