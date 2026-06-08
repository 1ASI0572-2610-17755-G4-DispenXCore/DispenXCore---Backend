using Backend_DispenXCore.Api.src.NotificacionesUsuario.Application.Interfaces;
using Backend_DispenXCore.Api.src.NotificacionesUsuario.Domain.Entities;
using Backend_DispenXCore.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend_DispenXCore.Api.src.NotificacionesUsuario.Infrastructure.Persistence;

public class NotificationRepository : INotificationRepository
{
    private readonly DispenXDbContext _context;
    public NotificationRepository(DispenXDbContext context) => _context = context;

    public async Task<List<Notification>> GetByUserIdAsync(Guid userId) =>
        await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();

    public async Task<Notification?> GetByIdAsync(Guid id) =>
        await _context.Notifications.FindAsync(id);

    public async Task AddAsync(Notification notification) =>
        await _context.Notifications.AddAsync(notification);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}