using Politico.Domain.Common.Interfaces;

namespace Politico.Domain.Entities.Articels
{
    public class ArticleLocale : IHasDeleteDate
    {
        public long Id { get; set; }

        public long ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public string Culture { get; set; } = null!;      // "en", "ka" и т.п.

        public string Title { get; set; } = null!;
        public string ShortSummary { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Slug { get; set; } = null!; 
        public DateTime? DeleteDate { get; set; }
    }
}
