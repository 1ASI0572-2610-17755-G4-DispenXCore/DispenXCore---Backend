using Backend_DispenXCore.Api.src.Dispositivos.Domain.Entities;

namespace Backend_DispenXCore.Api.src.Dispositivos.Application.Interfaces;

public interface IDeviceRepository
{
    Task<Device?> GetDeviceAsync();
    Task<Device?> GetDeviceByIdAsync(string id);
    Task UpdateDeviceAsync(Device device);
    Task AddDeviceAsync(Device device);
    Task PingAsync();
    Task<List<Firmware>> GetAllFirmwareAsync();
    Task<Firmware?> GetLatestFirmwareAsync();
    Task<Firmware?> GetFirmwareByIdAsync(string id);
    Task AddFirmwareAsync(Firmware firmware);
    Task SaveChangesAsync();
}