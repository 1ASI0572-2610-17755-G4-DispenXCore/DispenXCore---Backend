using Backend_DispenXCore.Api.src.AlertasStock.Domain.Entities;

namespace Backend_DispenXCore.Api.src.AlertasStock.Application.Interfaces;
public interface IAlertaRepository
{
    Task<List<Alerta>> GetAlertasPendientesAsync(Guid contenedorId);
    Task AddAsync(Alerta alerta);
    Task SaveChangesAsync();
}