using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Services;

namespace Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;

public class ObtenerDispensatorStatusQuery
{
    private readonly IDispenserRepository _repo;
    private readonly NextDispenseCalculator _calculator;

    public ObtenerDispensatorStatusQuery(IDispenserRepository repo, NextDispenseCalculator calculator)
    {
        _repo = repo;
        _calculator = calculator;
    }

    public async Task<object?> Execute(int dispensatorId)
    {
        var status = await _repo.GetStatusAsync(dispensatorId);
        if (status == null) return null;

        var activeSchedules = await _repo.GetActiveSchedulesAsync(dispensatorId);
        var next = _calculator.CalculateNextDispenseAt(activeSchedules, DateTime.Now);
        status.SetNextDispenseAt(next);

        // Calcular dailyTotal
        var today = DateTime.Today;
        var eventsToday = await _repo.GetEventsAsync(dispensatorId, today, today.AddDays(1).AddTicks(-1), null);
        var dailyTotal = eventsToday.Sum(e => e.AmountDispensed);
        status.ActualizarDailyTotal(dailyTotal);

        return new {
            id = status.Id,
            dispensatorId = status.DispensatorId,
            isActive = status.IsActive,
            currentCapacity = status.CurrentCapacity,
            maxCapacity = status.MaxCapacity,
            dailyTotal = status.DailyTotal,
            nextDispenseAt = status.NextDispenseAt
        };
    }
}