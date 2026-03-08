namespace NestIQ.DeviceRegistry.Application.Interfaces;

using NestIQ.DeviceRegistry.Domain.Entities;

public interface IDeviceRepository
{
    Task<bool> ExistsAsync(Guid homeId, string name);
    Task AddAsync(Device device);
}