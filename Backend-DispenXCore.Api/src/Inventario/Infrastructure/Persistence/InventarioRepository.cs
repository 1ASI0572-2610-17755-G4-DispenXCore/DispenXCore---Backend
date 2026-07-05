using Backend_DispenXCore.Api.src.Inventario.Application.Interfaces;
using Backend_DispenXCore.Api.src.Inventario.Domain.Entities;
using Backend_DispenXCore.Api.src.Inventario.Domain.ValueObjects;
using Backend_DispenXCore.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend_DispenXCore.Api.src.Inventario.Infrastructure.Persistence;
public class InventarioRepository : IInventarioRepository
{
    private readonly DispenXDbContext _context;
    public InventarioRepository(DispenXDbContext context) => _context = context;

    public async Task<Contenedor?> GetByIdAsync(Guid id) =>
        await _context.Contenedores.FindAsync(id);
    public async Task<List<Contenedor>> GetAllAsync() =>
        await _context.Contenedores.ToListAsync();
    public async Task AddAsync(Contenedor contenedor) =>
        await _context.Contenedores.AddAsync(contenedor);
    public async Task AddDatoSensorAsync(DatoSensor dato) =>
        await _context.DatosSensor.AddAsync(dato);

    public async Task<Guid> ResolveContainerIdAsync(string? deviceId, Guid? contenedorId)
    {
        if (contenedorId.HasValue && contenedorId.Value != Guid.Empty)
            return contenedorId.Value;

        if (!string.IsNullOrWhiteSpace(deviceId) && Guid.TryParse(deviceId, out var parsedDeviceId))
        {
            var byDeviceId = await _context.Contenedores.FindAsync(parsedDeviceId);
            if (byDeviceId != null)
                return byDeviceId.Id;
        }

        var existingContainer = await _context.Contenedores.OrderBy(c => c.Id).FirstOrDefaultAsync();
        if (existingContainer != null)
            return existingContainer.Id;

        var createdContainer = Contenedor.Crear(new TipoGrano("No especificado"), new Capacidad(1000, "kg"));
        await _context.Contenedores.AddAsync(createdContainer);
        await _context.SaveChangesAsync();
        return createdContainer.Id;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}