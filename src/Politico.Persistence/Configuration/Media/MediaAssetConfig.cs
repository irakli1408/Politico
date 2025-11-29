using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.Media;

namespace Politico.Persistence.Configuration.Media
{
    public class MediaAssetConfig : IEntityTypeConfiguration<MediaAsset>
    {
        public void Configure(EntityTypeBuilder<MediaAsset> b)
        {
            b.ToTable("MediaAssets");

            b.HasKey(x => x.Id);

            b.Property(x => x.OriginalFileName).HasMaxLength(255).IsRequired();
            b.Property(x => x.StoredPath).HasMaxLength(500).IsRequired();
            b.Property(x => x.ContentType).HasMaxLength(150).IsRequired();
            b.Property(x => x.Extension).HasMaxLength(20).IsRequired();

            b.HasMany(x => x.Attachments)
                .WithOne(x => x.MediaAsset)
                .HasForeignKey(x => x.MediaAssetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
