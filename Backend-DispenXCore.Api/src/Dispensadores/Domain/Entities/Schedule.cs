using Backend_DispenXCore.Api.src.Dispensadores.Domain.ValueObjects;
using Backend_DispenXCore.Api.Shared.Kernel;

namespace Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;

public class Schedule : BaseEntity
{
    public int DispensatorId { get; private set; }
    public string Name { get; private set; }
    public SupplyType SupplyType { get; private set; }
    public int Amount { get; private set; } // gramos
    public TimeSpan ScheduledTime { get; private set; }
    public FrequencyDays FrequencyDays { get; private set; }
    public bool SmartRefill { get; private set; }
    public bool IsActive { get; private set; }

    private Schedule() { }
    public Schedule(int dispensatorId, string name, SupplyType supplyType, int amount,
        TimeSpan scheduledTime, FrequencyDays frequencyDays, bool smartRefill)
    {
        DispensatorId = dispensatorId;
        Name = name;
        SupplyType = supplyType;
        Amount = amount;
        ScheduledTime = scheduledTime;
        FrequencyDays = frequencyDays;
        SmartRefill = smartRefill;
        IsActive = true;
    }

    public void Update(string name, SupplyType supplyType, int amount,
        TimeSpan scheduledTime, FrequencyDays frequencyDays, bool smartRefill, bool isActive)
    {
        Name = name;
        SupplyType = supplyType;
        Amount = amount;
        ScheduledTime = scheduledTime;
        FrequencyDays = frequencyDays;
        SmartRefill = smartRefill;
        IsActive = isActive;
    }

    public void ToggleActive() => IsActive = !IsActive;
}