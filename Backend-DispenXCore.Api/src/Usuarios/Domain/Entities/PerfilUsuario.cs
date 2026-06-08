using Backend_DispenXCore.Api.Shared.Kernel;

namespace Backend_DispenXCore.Api.src.Usuarios.Domain.Entities;
public class PerfilUsuario : BaseEntity
{
    public Guid UserId { get; private set; }
    public string NombreCompleto { get; private set; }
    public Guid? DispensadorId { get; private set; }
    public Dispensador? Dispensador { get; private set; }

    public PerfilUsuario(Guid userId, string nombreCompleto)
    {
        UserId = userId;
        NombreCompleto = nombreCompleto;
    }

    public void VincularDispensador(Guid dispensadorId) => DispensadorId = dispensadorId;
}