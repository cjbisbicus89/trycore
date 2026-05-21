namespace EVM.ProjectManagement.IntegrationTests.Activities;

using System.Net;
using System.Net.Http.Json;
using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.IntegrationTests.Fixtures;
using Xunit;

public sealed class ActivitiesControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient client;

    public ActivitiesControllerTests(ApiWebApplicationFactory factory)
    {
        this.client = factory.CreateClient();
    }

    [Fact]
    public async Task GetByProjectIdReturnsOk()
    {
        // Act
        var response = await this.client.GetAsync($"/api/activities/project/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdReturnsNotFoundCuandoActividadNoExiste()
    {
        // Act
        var response = await this.client.GetAsync($"/api/activities/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateReturnsNotFoundCuandoProyectoNoExiste()
    {
        // Arrange
        var request = new CreateActivityRequest(Guid.NewGuid(), "Test Activity", 1000, 50, 25, 300);

        // Act
        var response = await this.client.PostAsJsonAsync("/api/activities", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
