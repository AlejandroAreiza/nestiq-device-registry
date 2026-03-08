using NestIQ.DeviceRegistry.Application.Interfaces;
using NestIQ.DeviceRegistry.Domain.Entities;

namespace NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;

public class RegisterDeviceHandler
{
    private readonly IDeviceRepository _repository;

    public RegisterDeviceHandler(IDeviceRepository repository)
    {
        _repository = repository;
    }


    public async Task<RegisterDeviceResult> RegisterAsync(RegisterDeviceCommand command)
    {
        var deviceType = command.GetDeviceType(); // validates enum here

        var exists = await _repository.ExistsAsync(command.HomeId, command.Name);

        if (exists)
            throw new InvalidOperationException(
                $"A device with name '{command.Name}' already exists in this home.");

        var device = Device.Create(command.Name, deviceType, command.HomeId);

        await _repository.RegisterAsync(device);

        return new RegisterDeviceResult
        {
            Id = device.Id,
            Name = device.Name,
            Type = device.Type,
            Status = device.Status,
            HomeId = device.HomeId,
            CreatedAt = device.CreatedAt
        };
    }
}