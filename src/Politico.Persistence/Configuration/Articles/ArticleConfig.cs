using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.Articels;

namespace Politico.Persistence.Configuration.Articles
{
    public class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> b)
        {
            b.ToTable("Articles");

            b.HasKey(x => x.Id);

            b.Property(x => x.Category)
                .HasConversion<int>()
                .IsRequired();

            b.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            b.Property(x => x.IsActive)
                .IsRequired();

            b.Property(x => x.PublishDate)
                .IsRequired();

            b.Property(x => x.IsFeatured)
                .IsRequired();

            b.Property(x => x.PriorityScore)
                .IsRequired();

            b.Property(x => x.CreateDate)
                .IsRequired();

            // связи
            b.HasMany(x => x.Locales)
                .WithOne(l => l.Article)
                .HasForeignKey(l => l.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            // индексы
            b.HasIndex(x => new { x.Category, x.PublishDate });
            b.HasIndex(x => new { x.IsActive, x.PublishDate });
            b.HasIndex(x => new { x.IsFeatured, x.PriorityScore });

            // soft-delete фильтр
            b.HasQueryFilter(x => x.DeleteDate == null);
        }
    }
}
