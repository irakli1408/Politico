using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.Donors;

namespace Politico.Persistence.Configuration.Donors
{
    public class DonorLocaleConfig : IEntityTypeConfiguration<DonorLocale>
    {
        public void Configure(EntityTypeBuilder<DonorLocale> b)
        {
            b.ToTable("DonorLocales");

            b.HasKey(x => x.Id);

            b.Property(x => x.Culture)
                .HasMaxLength(5)
                .IsRequired();

            b.Property(x => x.Name)
                .HasMaxLength(300)
                .IsRequired();

            b.Property(x => x.Description)
                .HasMaxLength(2000);

            b.HasOne(x => x.Donor)
                .WithMany(x => x.Locales)
                .HasForeignKey(x => x.DonorId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => new { x.DonorId, x.Culture })
                .IsUnique();
        }
    }
}