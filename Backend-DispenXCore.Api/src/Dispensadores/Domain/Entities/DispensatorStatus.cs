namespace Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;

public class DispensatorStatus
{
    public int Id { get; private set; }
    public int DispensatorId { get; private set; }
    public bool IsActive { get; private set; }
    public int CurrentCapacity { get; private set; }
    public int MaxCapacity { get; private set; }
    public int DailyTotal { get; private set; }
    public DateTime NextDispenseAt { get; private set; }  // calculado en cada consulta

    private DispensatorStatus() { }
    public DispensatorStatus(int dispensatorId, int maxCapacity)
    {
        DispensatorId = dispensatorId;
        MaxCapacity = maxCapacity;
        CurrentCapacity = maxCapacity;
        IsActive = true;
    }

    public void ActualizarCapacidad(int amountDispensed)
    {
        CurrentCapacity -= amountDispensed;
    }

    public void ActualizarDailyTotal(int dailyTotal)
    {
        DailyTotal = dailyTotal;
    }

    public void SetNextDispenseAt(DateTime next)
    {
        NextDispenseAt = next;
    }
}