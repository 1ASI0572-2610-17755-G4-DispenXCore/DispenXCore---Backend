namespace Backend_DispenXCore.Api.src.Dispositivos.Domain.Entities;

public class Device
{
    public string Id { get; private set; }   // ej: 'DP-HOGAR-RADC'
    public string Name { get; private set; }
    public string Model { get; private set; }
    public string Location { get; private set; }
    public string SerialNumber { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public DateTime LastSeen { get; private set; }

    private Device() { }
    public Device(string id, string name, string model, string location, string serialNumber)
    {
        Id = id;
        Name = name;
        Model = model;
        Location = location;
        SerialNumber = serialNumber;
        RegisteredAt = DateTime.UtcNow;
        LastSeen = DateTime.UtcNow;
    }

    public void Update(string name, string location)
    {
        Name = name;
        Location = location;
    }

    public void Ping() => LastSeen = DateTime.UtcNow;
}