using Backend_DispenXCore.Api.Shared.Kernel;

namespace Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;

public enum DispensatorState { active, inactive }

public class Dispensator
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public DispensatorState Status { get; private set; }
    public int MaxCapacity { get; private set; } // gramos

    private Dispensator() { }

    public Dispensator(string name, int maxCapacity)
    {
        Name = name;
        MaxCapacity = maxCapacity;
        Status = DispensatorState.active;
    }

    // Nuevo constructor que acepta estado
    public Dispensator(string name, int maxCapacity, DispensatorState status)
    {
        Name = name;
        MaxCapacity = maxCapacity;
        Status = status;
    }
}