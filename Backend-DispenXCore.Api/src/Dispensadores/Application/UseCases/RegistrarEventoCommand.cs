using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;

namespace Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;

public class RegistrarEventoCommand
{
    private readonly IDispenserRepository _repo;
    public RegistrarEventoCommand(IDispenserRepository repo) => _repo = repo;

    public async Task Execute(int dispensatorId, int? scheduleId, string trigger,
        string? supplyType, int amountDispensed, DateTime dispensedAt)
    {
        var evento = new DispenserEvent(dispensatorId, scheduleId,
            Enum.Parse<EventTrigger>(trigger),
            supplyType != null ? Enum.Parse<EventSupplyType>(supplyType) : null,
            amountDispensed, dispensedAt);
        await _repo.AddEventAsync(evento);
        await _repo.SaveChangesAsync();
    }
}