namespace Politico.Application.DTO.ArticeleModuls
{
    public sealed class ArticleLocaleDto
    {
        public long? Id { get; set; }          // null при создании
        public string Culture { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ShortSummary { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}
