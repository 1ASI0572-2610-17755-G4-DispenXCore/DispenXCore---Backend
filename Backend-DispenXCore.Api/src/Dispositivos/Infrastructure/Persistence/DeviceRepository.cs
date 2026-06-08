using Backend_DispenXCore.Api.src.Dispositivos.Application.Interfaces;
using Backend_DispenXCore.Api.src.Dispositivos.Domain.Entities;
using Backend_DispenXCore.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend_DispenXCore.Api.src.Dispositivos.Infrastructure.Persistence;

public class DeviceRepository : IDeviceRepository
{
    private readonly DispenXDbContext _context;
    public DeviceRepository(DispenXDbContext context) => _context = context;

    public async Task<Device?> GetDeviceAsync() =>
        await _context.Devices.FirstOrDefaultAsync();

    public async Task UpdateDeviceAsync(Device device) =>
        _context.Devices.Update(device);

    public async Task PingAsync()
    {
        var device = await GetDeviceAsync();
        if (device != null)
        {
            device.Ping();
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Firmware>> GetAllFirmwareAsync() =>
        await _context.Firmwares.ToListAsync();

    public async Task<Firmware?> GetLatestFirmwareAsync() =>
        await _context.Firmwares.FirstOrDefaultAsync(f => f.IsLatest);

    public async Task<Firmware?> GetFirmwareByIdAsync(string id) =>
        await _context.Firmwares.FindAsync(id);

    public async Task AddFirmwareAsync(Firmware firmware) =>
        await _context.Firmwares.AddAsync(firmware);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}