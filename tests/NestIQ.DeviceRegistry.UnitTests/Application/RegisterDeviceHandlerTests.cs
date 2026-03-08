namespace NestIQ.DeviceRegistry.UnitTests.Application;

using FluentAssertions;
using Moq;
using NestIQ.DeviceRegistry.Application.Interfaces;
using NestIQ.DeviceRegistry.Application.UseCases;
using NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;
using NestIQ.DeviceRegistry.Domain.Entities;
using NestIQ.DeviceRegistry.Domain.Enums;

public class RegisterDeviceHandlerTests
{
    private readonly Mock<IDeviceRepository> _repositoryMock;
    private readonly DeviceHandler _handler;
    public RegisterDeviceHandlerTests()
    {
        _repositoryMock = new Mock<IDeviceRepository>();
        _handler = new DeviceHandler(_repositoryMock.Object);
    }

    // AC1: A device can be registered with a name, type and home ID
    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnCreatedDevice()
    {
        // Arrange
        var command = new RegisterDeviceCommand(
            Name: "Living Room Light",
            Type: "Light",
            HomeId: Guid.NewGuid()
        );

        _repositoryMock
            .Setup(r => r.ExistsAsync(command.HomeId, command.Name))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.RegisterAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Type.Should().Be(DeviceType.Light);
        result.HomeId.Should().Be(command.HomeId);
        result.Status.Should().Be(DeviceStatus.Active);
    }

    // AC4: Two devices in the same home cannot have the same name
    [Fact]
    public async Task Handle_WhenDeviceNameAlreadyExistsInHome_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new RegisterDeviceCommand(
            Name: "Living Room Light",
            Type: "Light",
            HomeId: Guid.NewGuid()
        );

        _repositoryMock
            .Setup(r => r.ExistsAsync(command.HomeId, command.Name))
            .ReturnsAsync(true);

        // Act
        var act = () => _handler.RegisterAsync(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }

    // AC1: Device should be saved to the repository
    [Fact]
    public async Task Handle_WithValidCommand_ShouldSaveDeviceToRepository()
    {
        // Arrange
        var command = new RegisterDeviceCommand(
            Name: "Front Door Camera",
            Type: "Camera",
            HomeId: Guid.NewGuid()
        );

        _repositoryMock
            .Setup(r => r.ExistsAsync(command.HomeId, command.Name))
            .ReturnsAsync(false);

        // Act
        await _handler.RegisterAsync(command);

        // Assert
        _repositoryMock.Verify(
            r => r.AddAsync(It.Is<Device>(d =>
                d.Name == command.Name &&
                d.HomeId == command.HomeId)),
            Times.Once);
    }


}