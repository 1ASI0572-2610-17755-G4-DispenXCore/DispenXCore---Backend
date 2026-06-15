namespace Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;

public enum EventTrigger { app, manual }
public enum EventSupplyType { RICE, LENTILS, BEANS, CORN, OTHER }

public class DispenserEvent
{
    public int Id { get; private set; }
    public int DispensatorId { get; private set; }
    public int? ScheduleId { get; private set; }
    public EventTrigger Trigger { get; private set; }
    public EventSupplyType? SupplyType { get; private set; }
    public int AmountDispensed { get; private set; }
    public DateTime DispensedAt { get; private set; }

    private DispenserEvent() { }
    public DispenserEvent(int dispensatorId, int? scheduleId, EventTrigger trigger,
        EventSupplyType? supplyType, int amountDispensed, DateTime dispensedAt)
    {
        DispensatorId = dispensatorId;
        ScheduleId = scheduleId;
        Trigger = trigger;
        SupplyType = supplyType;
        AmountDispensed = amountDispensed;
        DispensedAt = dispensedAt;
    }
}