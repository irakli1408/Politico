using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Politico.Domain.Entities.ErrorLoger;

namespace Politico.Persistence.Configuration.Logger;

public class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
{
    public void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        builder.ToTable("ErrorLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.Level)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(x => x.Message)
               .HasMaxLength(4000)
               .IsRequired();

        builder.Property(x => x.Path).HasMaxLength(500);
        builder.Property(x => x.Method).HasMaxLength(10);
        builder.Property(x => x.UserId).HasMaxLength(100);
        builder.Property(x => x.UserIp).HasMaxLength(50);
        builder.Property(x => x.UserAgent).HasMaxLength(400);
        builder.Property(x => x.AcceptedLanguage).HasMaxLength(20);
    }
}
