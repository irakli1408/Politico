using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.AboutOrg;

namespace Politico.Persistence.Configuration.AboutOrg
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.ToTable("TeamMembers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.OrderIndex)
                .IsRequired();

            // Один TeamMember → много локалей
            builder.HasMany(x => x.Locales)
                .WithOne(l => l.TeamMember)
                .HasForeignKey(l => l.TeamMemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
