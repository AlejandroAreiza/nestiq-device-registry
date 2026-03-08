using NestIQ.DeviceRegistry.Domain.Enums;

namespace NestIQ.DeviceRegistry.Application.UseCases.Device;

public record RegisterDeviceResult
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DeviceType Type { get; init; }
    public DeviceStatus Status { get; init; }
    public Guid HomeId { get; init; }
    public DateTime CreatedAt { get; init; }
}