namespace NestIQ.DeviceRegistry.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using NestIQ.DeviceRegistry.Domain.Entities;

public class DeviceRegistryDbContext : DbContext
{
    public DeviceRegistryDbContext(DbContextOptions<DeviceRegistryDbContext> options)
        : base(options) { }

    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeviceRegistryDbContext).Assembly);
    }
}