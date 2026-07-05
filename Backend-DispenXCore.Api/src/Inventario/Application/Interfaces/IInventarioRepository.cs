using Backend_DispenXCore.Api.src.Inventario.Domain.Entities;

namespace Backend_DispenXCore.Api.src.Inventario.Application.Interfaces;
public interface IInventarioRepository
{
    Task<Contenedor?> GetByIdAsync(Guid id);
    Task<List<Contenedor>> GetAllAsync();
    Task AddAsync(Contenedor contenedor);
    Task AddDatoSensorAsync(DatoSensor dato);
    Task<Guid> ResolveContainerIdAsync(string? deviceId, Guid? contenedorId);
    Task SaveChangesAsync();
}