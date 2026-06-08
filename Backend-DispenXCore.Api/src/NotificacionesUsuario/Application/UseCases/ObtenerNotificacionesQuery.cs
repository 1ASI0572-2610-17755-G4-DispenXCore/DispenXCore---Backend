using Backend_DispenXCore.Api.src.NotificacionesUsuario.Application.Interfaces;
using Backend_DispenXCore.Api.src.NotificacionesUsuario.Domain.Entities;

namespace Backend_DispenXCore.Api.src.NotificacionesUsuario.Application.UseCases;
public class ObtenerNotificacionesQuery
{
    private readonly INotificationRepository _repo;
    public ObtenerNotificacionesQuery(INotificationRepository repo) => _repo = repo;
    public async Task<List<Notification>> Execute(Guid userId) => await _repo.GetByUserIdAsync(userId);
}

// MarcarNotificacionesCommand.cs
public class MarcarNotificacionesCommand
{
    private readonly INotificationRepository _repo;
    public MarcarNotificacionesCommand(INotificationRepository repo) => _repo = repo;

    public async Task MarkAsRead(Guid id)
    {
        var notif = await _repo.GetByIdAsync(id);
        if (notif != null)
        {
            notif.MarkAsRead();
            await _repo.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsRead(Guid userId)
    {
        var notifs = await _repo.GetByUserIdAsync(userId);
        foreach (var n in notifs) n.MarkAsRead();
        await _repo.SaveChangesAsync();
    }
}