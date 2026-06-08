using Backend_DispenXCore.Api.src.NotificacionesUsuario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.NotificacionesUsuario.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.UserId).IsRequired();
        builder.Property(n => n.Type).HasConversion<string>().HasMaxLength(10);
        builder.Property(n => n.Title).HasMaxLength(200);
        builder.Property(n => n.Time).HasMaxLength(50);
        builder.Property(n => n.Message).HasColumnType("text");
        builder.Property(n => n.Action).HasMaxLength(100);
    }
}