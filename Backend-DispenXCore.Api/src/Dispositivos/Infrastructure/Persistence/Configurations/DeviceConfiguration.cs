using Backend_DispenXCore.Api.src.Dispositivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.Dispositivos.Infrastructure.Persistence.Configurations;
public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("devices");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever(); // el Id se asigna manualmente
        builder.Property(d => d.Name).HasMaxLength(100);
        builder.Property(d => d.Model).HasMaxLength(100);
        builder.Property(d => d.Location).HasMaxLength(200);
        builder.Property(d => d.SerialNumber).HasMaxLength(100);
    }
}