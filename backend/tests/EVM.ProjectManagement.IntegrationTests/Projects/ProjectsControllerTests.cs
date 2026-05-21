using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace EVM.ProjectManagement.IntegrationTests.Projects;

public sealed class ProjectsControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProjectsControllerTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/projects");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated()
    {
        // Arrange
        var request = new CreateProjectRequest("Test Project", "Test Description");

        // Act
        var response = await _client.PostAsJsonAsync("/api/projects", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_CuandoProyectoNoExiste()
    {
        // Act
        var response = await _client.GetAsync($"/api/projects/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
