using Backend_DispenXCore.Api.src.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.Usuarios.Infrastructure.Persistence.Configurations;
public class DispensadorConfiguration : IEntityTypeConfiguration<Dispensador>
{
    public void Configure(EntityTypeBuilder<Dispensador> builder)
    {
        builder.ToTable("dispensadores");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Codigo).HasMaxLength(50).IsRequired();
        builder.HasIndex(d => d.Codigo).IsUnique();
    }
}