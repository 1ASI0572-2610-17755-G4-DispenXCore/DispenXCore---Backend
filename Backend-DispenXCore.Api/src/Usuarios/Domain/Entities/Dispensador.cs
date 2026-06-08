namespace Backend_DispenXCore.Api.src.Usuarios.Domain.Entities;
public class Dispensador : Shared.Kernel.BaseEntity
{
    public string Codigo { get; private set; }
    public List<PerfilUsuario> Perfiles { get; private set; } = new();
    public Dispensador(string codigo) => Codigo = codigo;
}