using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.Donors;

namespace Politico.Persistence.Configuration.Donors
{
    public class DonorConfig : IEntityTypeConfiguration<Donor>
    {
        public void Configure(EntityTypeBuilder<Donor> b)
        {
            b.ToTable("Donors");

            b.HasKey(x => x.Id);

            b.Property(x => x.IsActive)
                .IsRequired();

            b.Property(x => x.WebsiteUrl)
                .HasMaxLength(500);

            b.Property(x => x.LogoFilePath)
                .HasMaxLength(500);

            b.HasMany(x => x.Locales)
                .WithOne(x => x.Donor)
                .HasForeignKey(x => x.DonorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}