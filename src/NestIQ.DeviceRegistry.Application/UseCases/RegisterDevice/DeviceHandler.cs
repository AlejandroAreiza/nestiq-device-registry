namespace NestIQ.DeviceRegistry.Application.UseCases;

using NestIQ.DeviceRegistry.Application.Interfaces;
using NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;
using NestIQ.DeviceRegistry.Domain.Entities;

public class DeviceHandler
{
    private readonly IDeviceRepository _repository;

    public DeviceHandler(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task<RegisterDeviceResult> RegisterAsync(RegisterDeviceCommand command)
    {
        var deviceType = command.GetDeviceType();

        var exists = await _repository.ExistsAsync(command.HomeId, command.Name);

        if (exists)
            throw new InvalidOperationException(
                $"A device with name '{command.Name}' already exists in this home.");

        var device = Device.Create(command.Name, deviceType, command.HomeId);

        await _repository.AddAsync(device);

        return MapToResult(device);
    }

    public async Task<RegisterDeviceResult?> GetAsync(Guid id)
    {
        var device = await _repository.GetByIdAsync(id);

        if (device is null)
            return null;

        return MapToResult(device);
    }

    private static RegisterDeviceResult MapToResult(Device device) =>
        new RegisterDeviceResult
        {
            Id = device.Id,
            Name = device.Name,
            Type = device.Type,
            Status = device.Status,
            HomeId = device.HomeId,
            CreatedAt = device.CreatedAt
        };
}