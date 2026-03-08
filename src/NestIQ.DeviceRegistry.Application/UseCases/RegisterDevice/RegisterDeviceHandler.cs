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


    public async Task<RegisterDeviceResult> HandleAsync(RegisterDeviceCommand command)
    {
        var exists = await _repository.ExistsAsync(command.HomeId, command.Name);

        if (exists)
            throw new InvalidOperationException(
                $"A device with name '{command.Name}' already exists in this home.");

        var device = Device.Create(command.Name, command.Type, command.HomeId);

        await _repository.AddAsync(device);
        
        return new RegisterDeviceResult(
            Id: device.Id,
            Name: device.Name,
            Type: device.Type,
            Status: device.Status,
            HomeId: device.HomeId,
            CreatedAt: device.CreatedAt
        );
 
    }
}