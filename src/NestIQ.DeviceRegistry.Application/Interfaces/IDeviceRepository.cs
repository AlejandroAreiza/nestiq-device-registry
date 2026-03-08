namespace NestIQ.DeviceRegistry.Application.Interfaces;

using NestIQ.DeviceRegistry.Domain.Entities;

public interface IDeviceRepository
{
    Task<bool> ExistsAsync(Guid homeId, string name);
    Task RegisterAsync(Device device);
}