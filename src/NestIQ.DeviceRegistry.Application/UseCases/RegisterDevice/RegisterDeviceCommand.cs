namespace NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;

using NestIQ.DeviceRegistry.Domain.Enums;

public record RegisterDeviceCommand(
    string Name,
    string Type,
    Guid HomeId
)
{
    public DeviceType GetDeviceType()
    {
        if (!Enum.TryParse<DeviceType>(Type, ignoreCase: true, out var deviceType))
            throw new ArgumentException(
                $"Invalid device type '{Type}'. Accepted values: {string.Join(", ", Enum.GetNames<DeviceType>())}.");

        return deviceType;
    }
}