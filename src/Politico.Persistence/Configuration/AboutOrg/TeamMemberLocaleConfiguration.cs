using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.AboutOrg;

namespace Politico.Persistence.Configuration.AboutOrg
{
    public class TeamMemberLocaleConfiguration : IEntityTypeConfiguration<TeamMemberLocale>
    {
        public void Configure(EntityTypeBuilder<TeamMemberLocale> builder)
        {
            builder.ToTable("TeamMemberLocales");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Culture)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Position)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Bio)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            // Уникальная локаль на TeamMember + Culture
            builder.HasIndex(x => new { x.TeamMemberId, x.Culture })
                .IsUnique();
        }
    }
}
