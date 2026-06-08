using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.ValueObjects;

namespace Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;

public class AdministrarSchedulesCommand
{
    private readonly IDispenserRepository _repo;
    public AdministrarSchedulesCommand(IDispenserRepository repo) => _repo = repo;

    public async Task<Schedule> Create(int dispensatorId, string name, string supplyType,
                                       int amount, string scheduledTime, List<int> frequencyDays, bool smartRefill)
    {
        var schedule = new Schedule(dispensatorId, name,
            Enum.Parse<SupplyType>(supplyType), amount,
            TimeSpan.Parse(scheduledTime), new FrequencyDays(frequencyDays), smartRefill);
        await _repo.AddScheduleAsync(schedule);
        await _repo.SaveChangesAsync();
        return schedule;
    }

    public async Task Update(int scheduleId, string name, string supplyType, int amount,
                             string scheduledTime, List<int> frequencyDays, bool smartRefill, bool isActive)
    {
        var schedule = await _repo.GetScheduleByIdAsync(scheduleId);
        if (schedule == null) throw new InvalidOperationException("Schedule no encontrado");
        schedule.Update(name, Enum.Parse<SupplyType>(supplyType), amount,
                        TimeSpan.Parse(scheduledTime), new FrequencyDays(frequencyDays), smartRefill, isActive);
        await _repo.UpdateScheduleAsync(schedule);
        await _repo.SaveChangesAsync();
    }

    public async Task Delete(int scheduleId)
    {
        var schedule = await _repo.GetScheduleByIdAsync(scheduleId);
        if (schedule != null)
        {
            await _repo.DeleteScheduleAsync(schedule);
            await _repo.SaveChangesAsync();
        }
    }

    public async Task Toggle(int scheduleId)
    {
        var schedule = await _repo.GetScheduleByIdAsync(scheduleId);
        if (schedule == null) throw new InvalidOperationException("Schedule no encontrado");
        schedule.ToggleActive();
        await _repo.UpdateScheduleAsync(schedule);
        await _repo.SaveChangesAsync();
    }
}