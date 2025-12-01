using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.AboutOrg;

namespace Politico.Persistence.Configuration.AboutOrg
{
    public class AboutOrganizationConfiguration : IEntityTypeConfiguration<AboutOrganization>
    {
        public void Configure(EntityTypeBuilder<AboutOrganization> builder)
        {
            builder.ToTable("AboutOrganizations");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Locales)
                .WithOne(l => l.AboutOrganization)
                .HasForeignKey(l => l.AboutOrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
