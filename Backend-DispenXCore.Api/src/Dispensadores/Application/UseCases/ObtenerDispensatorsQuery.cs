using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;

namespace Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;

public class ObtenerDispensatorsQuery
{
    private readonly IDispenserRepository _repo;
    public ObtenerDispensatorsQuery(IDispenserRepository repo) => _repo = repo;
    public async Task<List<object>> Execute()
    {
        var list = await _repo.GetAllAsync();
        return list.Select(d => new { d.Id, d.Name, status = d.Status.ToString(), d.MaxCapacity }).ToList<object>();
    }
}