using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.AboutOrg;

namespace Politico.Persistence.Configuration.AboutOrg
{
    public class AboutOrganizationLocaleConfiguration : IEntityTypeConfiguration<AboutOrganizationLocale>
    {
        public void Configure(EntityTypeBuilder<AboutOrganizationLocale> builder)
        {
            builder.ToTable("AboutOrganizationLocales");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Culture)
                .IsRequired()
                .HasMaxLength(10); // "ka", "en-US" и т.п.

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.HasIndex(x => new { x.AboutOrganizationId, x.Culture })
                .IsUnique();
        }
    }
}
