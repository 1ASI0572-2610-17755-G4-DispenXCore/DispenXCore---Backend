using Backend_DispenXCore.Api.src.AlertasStock.Application.Interfaces;

namespace Backend_DispenXCore.Api.src.AlertasStock.Application.UseCases;
public class ObtenerAlertasQuery
{
    private readonly IAlertaRepository _repo;
    public ObtenerAlertasQuery(IAlertaRepository repo) => _repo = repo;

    public async Task<List<object>> Execute(Guid contenedorId)
    {
        var alertas = await _repo.GetAlertasPendientesAsync(contenedorId);
        return alertas.Select(a => new
        {
            a.Id,
            a.Grano,
            a.PorcentajeActual,
            a.UmbralDisparo,
            a.FechaCreacion,
            Enviada = a.Enviada
        }).ToList<object>();
    }
}