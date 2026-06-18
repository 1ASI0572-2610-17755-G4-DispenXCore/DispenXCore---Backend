using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_DispenXCore.Api.src.NotificacionesUsuario.Domain.Entities;

public enum NotificationType { ALERT, SUCCESS, INFO }

public class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Title { get; private set; }
    public string Time { get; private set; }   // texto legible
    public string Message { get; private set; }
    public string? Action { get; private set; }
    public bool Unread { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Notification() { }
    public Notification(Guid userId, NotificationType type, string title, string time, string message, string? action)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Type = type;
        Title = title;
        Time = time;
        Message = message;
        Action = action;
        Unread = true;
        CreatedAt = DateTime.UtcNow;
    }
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);
    
        // ── Fix GUID columns para MySQL ──────────────────────────────────────
        builder.Property(n => n.Id)
            .HasColumnType("char(36)");
        builder.Property(n => n.UserId)
            .HasColumnType("char(36)")
            .IsRequired();
    
        builder.Property(n => n.Type).HasConversion<string>().HasMaxLength(10);
        builder.Property(n => n.Title).HasMaxLength(200);
        builder.Property(n => n.Time).HasMaxLength(50);
        builder.Property(n => n.Message).HasColumnType("text");
        builder.Property(n => n.Action).HasMaxLength(100);
    }

    public void MarkAsRead() => Unread = false;
}