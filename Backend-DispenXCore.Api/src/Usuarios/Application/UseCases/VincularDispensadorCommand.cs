using Backend_DispenXCore.Api.src.Usuarios.Application.Interfaces;
using Backend_DispenXCore.Api.src.Usuarios.Domain.Entities;

namespace Backend_DispenXCore.Api.src.Usuarios.Application.UseCases;
public class VincularDispensadorCommand
{
    private readonly IPerfilRepository _repo;
    public VincularDispensadorCommand(IPerfilRepository repo) => _repo = repo;

    public async Task Execute(Guid userId, string codigoDispensador)
    {
        var perfil = await _repo.GetByUserIdAsync(userId);
        if (perfil == null) throw new InvalidOperationException("Perfil no encontrado");

        var dispensador = await _repo.GetDispensadorByCodigoAsync(codigoDispensador);
        if (dispensador == null)
        {
            dispensador = new Dispensador(codigoDispensador);
            await _repo.AddDispensadorAsync(dispensador);
        }

        perfil.VincularDispensador(dispensador.Id);
        await _repo.SaveChangesAsync();
    }
}