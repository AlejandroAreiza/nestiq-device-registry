namespace NestIQ.DeviceRegistry.IntegrationTests.Repositories;

using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NestIQ.DeviceRegistry.Domain.Entities;
using NestIQ.DeviceRegistry.Domain.Enums;
using NestIQ.DeviceRegistry.Infrastructure.Persistence;
using NestIQ.DeviceRegistry.Infrastructure.Persistence.Repositories;
using Testcontainers.PostgreSql;

public record DeviceRecord(
    Guid Id,
    string Name,
    string Type,
    string Status,
    Guid HomeId,
    DateTime CreatedAt
);

public class DeviceRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
    .WithDatabase("nestiq_device_registry_test")
    .WithUsername("postgres")
    .WithPassword("postgres")
    .Build();

    private DeviceRegistryDbContext _context = null!;
    private DeviceRepository _repository = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<DeviceRegistryDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        _context = new DeviceRegistryDbContext(options);
        await _context.Database.MigrateAsync();

        _repository = new DeviceRepository(_context);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task AddAsync_ShouldStoreEnumAsString()
    {
        // Arrange
        var device = Device.Create("Front Camera", DeviceType.Camera, Guid.NewGuid());

        // Act
        await _repository.AddAsync(device);

        // Assert
        await using var connection = new NpgsqlConnection(_container.GetConnectionString());
        var type = await connection.ExecuteScalarAsync<string>(
            @"SELECT ""Type"" FROM ""Devices"" WHERE ""Id"" = @Id",
            new { Id = device.Id });

        type.Should().Be("Camera");
        type.Should().NotBe("3");
    }

    [Fact]
    public async Task AddAsync_ShouldStoreTrimmedName()
    {
        // Arrange
        var device = Device.Create("  thermostat 1  ", DeviceType.Thermostat, Guid.NewGuid());

        // Act
        await _repository.AddAsync(device);

        // Assert
        await using var connection = new NpgsqlConnection(_container.GetConnectionString());
        var name = await connection.ExecuteScalarAsync<string>(
            @"SELECT ""Name"" FROM ""Devices"" WHERE ""Id"" = @Id",
            new { Id = device.Id });

        name.Should().Be("thermostat 1");
    }

    [Fact]
    public async Task ExistsAsync_WhenDeviceExists_ShouldReturnTrue()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var device = Device.Create("Kitchen Light", DeviceType.Light, homeId);
        await _repository.AddAsync(device);

        // Act
        var exists = await _repository.ExistsAsync(homeId, "Kitchen Light");

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WhenDeviceDoesNotExist_ShouldReturnFalse()
    {
        // Act
        var exists = await _repository.ExistsAsync(Guid.NewGuid(), "Non Existent Device");

        // Assert
        exists.Should().BeFalse();
    }
}