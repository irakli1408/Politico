using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.Media;

namespace Politico.Persistence.Configuration.Media
{
    public class MediaAttachmentConfig : IEntityTypeConfiguration<MediaAttachment>
    {
        public void Configure(EntityTypeBuilder<MediaAttachment> b)
        {
            b.ToTable("MediaAttachments");

            b.HasKey(x => x.Id);

            b.Property(x => x.OwnerKey).HasMaxLength(100).IsRequired();
            b.HasIndex(x => new { x.OwnerType, x.OwnerKey });
        }
    }
}
