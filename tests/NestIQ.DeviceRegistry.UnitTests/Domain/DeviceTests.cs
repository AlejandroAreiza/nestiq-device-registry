namespace NestIQ.DeviceRegistry.UnitTests.Domain;

using FluentAssertions;
using NestIQ.DeviceRegistry.Domain.Entities;
using NestIQ.DeviceRegistry.Domain.Enums;

public class DeviceTests
{
    //AC1: A device can be registered with a name, type and home ID
    [Fact]
    public void Create_WithValidData_ShouldCreateDevice()
    {
        // Arrange
        var name = "Living Room Thermostat";
        var type = DeviceType.Thermostat;
        var homeId = Guid.NewGuid();
        // Act
        var device = Device.Create(name, type, homeId);
        // Assert
        device.Should().NotBeNull();
        device.Id.Should().NotBeEmpty();
        device.Name.Should().Be(name);
        device.Type.Should().Be(type);
        device.HomeId.Should().Be(homeId);
    }

    // AC3: A device is created with status ACTIVE by default
    [Fact]
    public void Create_WithValidData_ShouldSetStatusToActive()
    {
        // Arrange
        var homeId = Guid.NewGuid();

        // Act
        var device = Device.Create("Front Door Lock", DeviceType.Lock, homeId);

        // Assert
        device.Status.Should().Be(DeviceStatus.Active);
    }

    // AC5: A device name cannot be empty
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        var homeId = Guid.NewGuid();

        // Act
        var act = () => Device.Create(invalidName, DeviceType.Light, homeId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Device name cannot be empty*");
    }

        // AC6: A home ID must be provided
    [Fact]
    public void Create_WithEmptyHomeId_ShouldThrowArgumentException()
    {
        // Act
        var act = () => Device.Create("Camera 1", DeviceType.Camera, Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Home ID must be provided*");
    }

    // AC1: CreatedAt is set by the system
    [Fact]
    public void Create_WithValidData_ShouldSetCreatedAt()
    {
        // Arrange
        var before = DateTime.UtcNow;

        // Act
        var device = Device.Create("Garden Light", DeviceType.Light, Guid.NewGuid());

        // Assert
        device.CreatedAt.Should().BeOnOrAfter(before)
            .And.BeOnOrBefore(DateTime.UtcNow);
    }
}