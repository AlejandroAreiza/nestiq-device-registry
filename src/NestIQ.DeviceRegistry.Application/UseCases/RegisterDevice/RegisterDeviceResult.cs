using NestIQ.DeviceRegistry.Domain.Enums;

namespace NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;

public record RegisterDeviceResult(
    Guid Id,
    string Name,
    DeviceType Type,
    DeviceStatus Status,
    Guid HomeId,
    DateTime CreatedAt
);