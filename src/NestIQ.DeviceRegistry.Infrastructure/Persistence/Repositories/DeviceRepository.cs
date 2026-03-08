namespace NestIQ.DeviceRegistry.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using NestIQ.DeviceRegistry.Application.Interfaces;
using NestIQ.DeviceRegistry.Domain.Entities;

public class DeviceRepository : IDeviceRepository
{
    private readonly DeviceRegistryDbContext _context;

    public DeviceRepository(DeviceRegistryDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid homeId, string name)
    {
        return await _context.Devices
            .AnyAsync(d => d.HomeId == homeId && d.Name == name);
    }

    public async Task RegisterAsync(Device device)
    {
        await _context.Devices.AddAsync(device);
        await _context.SaveChangesAsync();
    }
}