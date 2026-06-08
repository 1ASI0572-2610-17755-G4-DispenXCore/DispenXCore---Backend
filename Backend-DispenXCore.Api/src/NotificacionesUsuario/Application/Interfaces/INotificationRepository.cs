using Backend_DispenXCore.Api.src.NotificacionesUsuario.Domain.Entities;

namespace Backend_DispenXCore.Api.src.NotificacionesUsuario.Application.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserIdAsync(Guid userId);
    Task<Notification?> GetByIdAsync(Guid id);
    Task AddAsync(Notification notification);
    Task SaveChangesAsync();
}