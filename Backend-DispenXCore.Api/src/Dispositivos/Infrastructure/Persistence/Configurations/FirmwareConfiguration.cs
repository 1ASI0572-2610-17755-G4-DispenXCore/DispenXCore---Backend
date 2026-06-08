using Backend_DispenXCore.Api.src.Dispositivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.Dispositivos.Infrastructure.Persistence.Configurations;
public class FirmwareConfiguration : IEntityTypeConfiguration<Firmware>
{
    public void Configure(EntityTypeBuilder<Firmware> builder)
    {
        builder.ToTable("firmwares");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(f => f.Version).HasMaxLength(20);
        builder.Property(f => f.Changelog).HasColumnType("text");
        builder.Property(f => f.DownloadUrl).HasMaxLength(500);
    }
}