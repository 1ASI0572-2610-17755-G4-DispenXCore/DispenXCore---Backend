using Backend_DispenXCore.Api.src.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.Usuarios.Infrastructure.Persistence.Configurations;
public class PerfilUsuarioConfiguration : IEntityTypeConfiguration<PerfilUsuario>
{
    public void Configure(EntityTypeBuilder<PerfilUsuario> builder)
    {
        builder.ToTable("perfiles_usuarios");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.NombreCompleto).HasMaxLength(150);
        builder.Property(p => p.DispensadorId).IsRequired(false);
        builder.HasOne(p => p.Dispensador)
            .WithMany(d => d.Perfiles)
            .HasForeignKey(p => p.DispensadorId);
    }
}