using NestIQ.DeviceRegistry.Domain.Enums;

namespace NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;

public record RegisterDeviceCommand(
    string Name,
    DeviceType Type,
    Guid HomeId
);