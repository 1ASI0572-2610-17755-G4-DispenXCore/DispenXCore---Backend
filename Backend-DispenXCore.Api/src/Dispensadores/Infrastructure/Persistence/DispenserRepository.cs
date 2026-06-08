using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;
using Backend_DispenXCore.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend_DispenXCore.Api.src.Dispensadores.Infrastructure.Persistence;

public class DispenserRepository : IDispenserRepository
{
    private readonly DispenXDbContext _context;
    public DispenserRepository(DispenXDbContext context) => _context = context;

    public async Task<List<Dispensator>> GetAllAsync() =>
        await _context.Dispensators.ToListAsync();

    public async Task<Dispensator?> GetByIdAsync(int id) =>
        await _context.Dispensators.FindAsync(id);

    public async Task<DispensatorStatus?> GetStatusAsync(int dispensatorId) =>
        await _context.DispensatorStatuses.FirstOrDefaultAsync(s => s.DispensatorId == dispensatorId);

    public async Task<List<Schedule>> GetActiveSchedulesAsync(int dispensatorId) =>
        await _context.Schedules.Where(s => s.DispensatorId == dispensatorId && s.IsActive).ToListAsync();

    public async Task<Schedule?> GetScheduleByIdAsync(int id) =>
        await _context.Schedules.FindAsync(id);

    public async Task AddScheduleAsync(Schedule schedule) =>
        await _context.Schedules.AddAsync(schedule);

    public async Task UpdateScheduleAsync(Schedule schedule) =>
        _context.Schedules.Update(schedule);

    public async Task DeleteScheduleAsync(Schedule schedule) =>
        _context.Schedules.Remove(schedule);

    public async Task<List<DispenserEvent>> GetEventsAsync(int dispensatorId, DateTime? from, DateTime? to, string? supplyType)
    {
        var query = _context.DispenserEvents.Where(e => e.DispensatorId == dispensatorId);
        if (from.HasValue) query = query.Where(e => e.DispensedAt >= from.Value);
        if (to.HasValue) query = query.Where(e => e.DispensedAt <= to.Value);
        if (!string.IsNullOrEmpty(supplyType) && Enum.TryParse<EventSupplyType>(supplyType, out var st))
            query = query.Where(e => e.SupplyType == st);
        return await query.ToListAsync();
    }

    public async Task AddEventAsync(DispenserEvent dispenserEvent) =>
        await _context.DispenserEvents.AddAsync(dispenserEvent);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}