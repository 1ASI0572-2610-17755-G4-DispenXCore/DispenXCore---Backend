using Backend_DispenXCore.Api.src.AlertasStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.AlertasStock.Infrastructure.Persistence.Configurations;
public class AlertaConfiguration : IEntityTypeConfiguration<Alerta>
{
    public void Configure(EntityTypeBuilder<Alerta> builder)
    {
        builder.ToTable("alertas");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.ContenedorId);
        builder.Property(a => a.Grano).HasMaxLength(100);
        builder.Property(a => a.PorcentajeActual);
        builder.Property(a => a.UmbralDisparo);
        builder.Property(a => a.FechaCreacion);
        builder.Property(a => a.Enviada);
    }
}