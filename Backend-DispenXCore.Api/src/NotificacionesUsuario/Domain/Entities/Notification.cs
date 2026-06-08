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

    public void MarkAsRead() => Unread = false;
}