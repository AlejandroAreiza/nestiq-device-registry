namespace NestIQ.DeviceRegistry.ApiTests;

using System.Net;
using FluentAssertions;
using Refit;

public class DeviceSmokeTests
{
    private readonly IDeviceRegistryApi _api;

    public DeviceSmokeTests()
    {
        _api = RestService.For<IDeviceRegistryApi>("http://localhost:5283");
    }

    [Fact]
    public async Task RegisterAndGetDevice_SmokeTest_ShouldReturn201And200()
    {
        // Arrange
        var command = new RegisterDeviceCommand(
            Name: $"Smoke Test Device {Guid.NewGuid()}",
            Type: "Light",
            HomeId: Guid.NewGuid()
        );

        // Act — register
        var registerResponse = await _api.RegisterDeviceAsync(command);

        // Assert — service is alive, DB write works
        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var deviceId = registerResponse.Content!.Id;

        // Act — retrieve
        var getResponse = await _api.GetDeviceAsync(deviceId);

        // Assert — DB read works
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Content!.Id.Should().Be(deviceId);
        getResponse.Content.Name.Should().Be(command.Name);
    }
}