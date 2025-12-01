using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.Articels;

namespace Politico.Persistence.Configuration.Articles
{
    public class ArticleLocaleConfig : IEntityTypeConfiguration<ArticleLocale>
    {
        public void Configure(EntityTypeBuilder<ArticleLocale> b)
        {
            b.ToTable("ArticleLocales");

            b.HasKey(x => x.Id);

            b.Property(x => x.Culture)
                .HasMaxLength(10)
                .IsRequired();

            b.Property(x => x.Title)
                .IsRequired();

            b.Property(x => x.ShortSummary)
                .IsRequired();

            b.Property(x => x.Content)
                .IsRequired();

            b.Property(x => x.Slug)
                .HasMaxLength(256)
                .IsRequired();

            // 1 язык для статьи
            b.HasIndex(x => new { x.ArticleId, x.Culture })
                .IsUnique();

            // быстрый поиск по URL
            b.HasIndex(x => new { x.Culture, x.Slug })
                .IsUnique(); 
            
            b.HasQueryFilter(x => x.DeleteDate == null);

        }
    }
}
