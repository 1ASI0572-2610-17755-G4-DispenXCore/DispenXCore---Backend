using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;
using Backend_DispenXCore.Api.src.Dispositivos.Application.Interfaces;
using Backend_DispenXCore.Api.Shared.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;

public class RegistrarEventoCommand
{
    private readonly IDispenserRepository _repo;
    private readonly IDeviceRepository _deviceRepo;
    private readonly IEdgeClient _edgeClient;
    private readonly ILogger<RegistrarEventoCommand> _logger;

    public RegistrarEventoCommand(
        IDispenserRepository repo,
        IDeviceRepository deviceRepo,
        IEdgeClient edgeClient,
        ILogger<RegistrarEventoCommand> logger)
    {
        _repo = repo;
        _deviceRepo = deviceRepo;
        _edgeClient = edgeClient;
        _logger = logger;
    }

    public async Task Execute(int dispensatorId, int? scheduleId, string trigger,
        string? supplyType, decimal amountDispensed, DateTime dispensedAt)
    {
        var triggerEnum = Enum.Parse<EventTrigger>(trigger);
        var supplyEnum = supplyType != null ? (EventSupplyType?)Enum.Parse<EventSupplyType>(supplyType) : null;

        var evento = new DispenserEvent(dispensatorId, scheduleId,
            triggerEnum,
            supplyEnum,
            amountDispensed, dispensedAt);

        await _repo.AddEventAsync(evento);
        await _repo.SaveChangesAsync();

        if (triggerEnum == EventTrigger.app)
        {
            try
            {
                var dispensator = await _repo.GetByIdAsync(dispensatorId);
                if (dispensator != null)
                {
                    var device = await _deviceRepo.GetDeviceByIdAsync(dispensator.Name);
                    if (device == null)
                    {
                        device = await _deviceRepo.GetDeviceAsync();
                    }

                    string deviceId = device?.Id ?? dispensator.Name;
                    
                    _logger.LogInformation("[REGISTRAR-EVENTO-COMMAND] Propagating event trigger 'app' to Edge. DeviceId: {DeviceId}, SupplyType: {SupplyType}", deviceId, supplyType);
                    await _edgeClient.ActivateDispenserAsync(deviceId, supplyType);
                }
                else
                {
                    _logger.LogWarning("[REGISTRAR-EVENTO-COMMAND] Dispensator with ID {DispensatorId} not found. Cannot send activation command to Edge.", dispensatorId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[REGISTRAR-EVENTO-COMMAND] Error sending dispenser activation request to Edge service.");
            }
        }
    }
}