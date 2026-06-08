// ObtenerDeviceQuery.cs
using Backend_DispenXCore.Api.src.Dispositivos.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispositivos.Domain.Entities;

namespace Backend_DispenXCore.Api.src.Dispositivos.Application.UseCases;
public class ObtenerDeviceQuery
{
    private readonly IDeviceRepository _repo;
    public ObtenerDeviceQuery(IDeviceRepository repo) => _repo = repo;
    public async Task<object?> Execute() => await _repo.GetDeviceAsync();
}

// ActualizarDeviceCommand.cs
public class ActualizarDeviceCommand
{
    private readonly IDeviceRepository _repo;
    public ActualizarDeviceCommand(IDeviceRepository repo) => _repo = repo;
    public async Task Execute(string name, string location)
    {
        var device = await _repo.GetDeviceAsync();
        if (device == null) throw new InvalidOperationException("Dispositivo no encontrado");
        device.Update(name, location);
        await _repo.UpdateDeviceAsync(device);
        await _repo.SaveChangesAsync();
    }
}

// RegistrarPingCommand.cs
public class RegistrarPingCommand
{
    private readonly IDeviceRepository _repo;
    public RegistrarPingCommand(IDeviceRepository repo) => _repo = repo;
    public async Task Execute()
    {
        var device = await _repo.GetDeviceAsync();
        if (device == null) throw new InvalidOperationException("Dispositivo no encontrado");
        device.Ping();
        await _repo.UpdateDeviceAsync(device);
        await _repo.SaveChangesAsync();
    }
}

// FirmwareUseCases.cs (contiene varios)
public class FirmwareUseCases
{
    private readonly IDeviceRepository _repo;
    public FirmwareUseCases(IDeviceRepository repo) => _repo = repo;

    public async Task<List<Firmware>> GetAll() => await _repo.GetAllFirmwareAsync();
    public async Task<Firmware?> GetLatest() => await _repo.GetLatestFirmwareAsync();
    public async Task Install(string firmwareId)
    {
        // Simulación de instalación
        var fw = await _repo.GetFirmwareByIdAsync(firmwareId);
        if (fw == null) throw new InvalidOperationException("Firmware no encontrado");
        // Aquí se podría desencadenar el proceso
    }
}