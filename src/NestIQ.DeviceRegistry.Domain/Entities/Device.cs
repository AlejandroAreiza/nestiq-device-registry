namespace NestIQ.DeviceRegistry.Domain.Entities;

using NestIQ.DeviceRegistry.Domain.Enums;

public class Device
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DeviceType Type { get; set; }
    public DeviceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid HomeId { get; set; }

    private Device() {}

    public static Device Create(string name, DeviceType type, Guid homeId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name cannot be empty.", nameof(name));

        if (homeId == Guid.Empty)
            throw new ArgumentException("Home ID must be provided.", nameof(homeId));
        
        return new Device
        {
            Id = Guid.NewGuid(),
            Name = name,
            Type = type,
            Status = DeviceStatus.Active,
            HomeId = homeId,
            CreatedAt = DateTime.UtcNow
        };
    }
 
}