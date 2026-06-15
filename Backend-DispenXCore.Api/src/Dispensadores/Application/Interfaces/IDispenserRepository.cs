using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;

namespace Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;

public interface IDispenserRepository
{
    Task<List<Dispensator>> GetAllAsync();
    Task<Dispensator?> GetByIdAsync(int id);
    Task AddDispensatorAsync(Dispensator dispensator);
    Task<DispensatorStatus?> GetStatusAsync(int dispensatorId);
    Task AddDispensatorStatusAsync(DispensatorStatus status);
    Task<List<Schedule>> GetActiveSchedulesAsync(int dispensatorId);
    Task<Schedule?> GetScheduleByIdAsync(int id);
    Task AddScheduleAsync(Schedule schedule);
    Task UpdateScheduleAsync(Schedule schedule);
    Task DeleteScheduleAsync(Schedule schedule);
    Task<List<DispenserEvent>> GetEventsAsync(int dispensatorId, DateTime? from, DateTime? to, string? supplyType);
    Task AddEventAsync(DispenserEvent dispenserEvent);
    Task SaveChangesAsync();
}