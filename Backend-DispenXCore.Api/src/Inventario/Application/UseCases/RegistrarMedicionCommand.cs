using Backend_DispenXCore.Api.src.Inventario.Application.Interfaces;
using Backend_DispenXCore.Api.src.Inventario.Domain.Entities;

namespace Backend_DispenXCore.Api.src.Inventario.Application.UseCases;
public class RegistrarMedicionCommand
{
    private readonly IInventarioRepository _repo;
    public RegistrarMedicionCommand(IInventarioRepository repo) => _repo = repo;

    public async Task Execute(string? deviceId, Guid? contenedorId, double peso, double nivel, double flujo)
    {
        var resolvedContainerId = await _repo.ResolveContainerIdAsync(deviceId, contenedorId);
        var contenedor = await _repo.GetByIdAsync(resolvedContainerId);
        if (contenedor == null) throw new InvalidOperationException("Contenedor no encontrado");

        contenedor.ActualizarMedicion(peso, nivel);
        var dato = new DatoSensor(resolvedContainerId, peso, nivel, flujo);
        await _repo.AddDatoSensorAsync(dato);
        await _repo.SaveChangesAsync();
    }
}