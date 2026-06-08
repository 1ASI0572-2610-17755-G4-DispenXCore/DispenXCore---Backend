
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.Dispensadores.Infrastructure.Persistence.Configurations;

public class DispensatorConfiguration : IEntityTypeConfiguration<Dispensator>
{
    public void Configure(EntityTypeBuilder<Dispensator> builder)
    {
        builder.ToTable("dispensators");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).HasMaxLength(100).IsRequired();
        builder.Property(d => d.Status).HasConversion<string>().HasMaxLength(10);
        builder.Property(d => d.MaxCapacity).IsRequired();
    }
}