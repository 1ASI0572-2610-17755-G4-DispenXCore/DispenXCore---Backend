using Backend_DispenXCore.Api.src.AlertasStock.Application.Interfaces;
using Backend_DispenXCore.Api.src.AlertasStock.Domain.Entities;
using Backend_DispenXCore.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend_DispenXCore.Api.src.AlertasStock.Infrastructure.Persistence;
public class AlertaRepository : IAlertaRepository
{
    private readonly DispenXDbContext _context;
    public AlertaRepository(DispenXDbContext context) => _context = context;

    public async Task<List<Alerta>> GetAlertasPendientesAsync(Guid contenedorId) =>
        await _context.Alertas.Where(a => a.ContenedorId == contenedorId).ToListAsync();
    public async Task AddAsync(Alerta alerta) =>
        await _context.Alertas.AddAsync(alerta);
    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}