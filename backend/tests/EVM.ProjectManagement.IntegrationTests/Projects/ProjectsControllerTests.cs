namespace EVM.ProjectManagement.IntegrationTests.Projects;

using System.Net;
using System.Net.Http.Json;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.IntegrationTests.Fixtures;
using Xunit;

public sealed class ProjectsControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient client;

    public ProjectsControllerTests(ApiWebApplicationFactory factory)
    {
        this.client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllReturnsOk()
    {
        // Act
        var response = await this.client.GetAsync("/api/projects");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateReturnsCreated()
    {
        // Arrange
        var request = new CreateProjectRequest("Test Project", "Test Description");

        // Act
        var response = await this.client.PostAsJsonAsync("/api/projects", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdReturnsNotFoundCuandoProyectoNoExiste()
    {
        // Act
        var response = await this.client.GetAsync($"/api/projects/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdReturnsOkCuandoProyectoExiste()
    {
        // Arrange
        var createRequest = new CreateProjectRequest("Test Project", "Test Description");
        var createResponse = await this.client.PostAsJsonAsync("/api/projects", createRequest);
        var project = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        // Act
        var response = await this.client.GetAsync($"/api/projects/{project!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReturnsOkCuandoProyectoExiste()
    {
        // Arrange
        var createRequest = new CreateProjectRequest("Test Project", "Test Description");
        var createResponse = await this.client.PostAsJsonAsync("/api/projects", createRequest);
        var project = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var updateRequest = new UpdateProjectRequest("Updated Project", "Updated Description");

        // Act
        var response = await this.client.PutAsJsonAsync($"/api/projects/{project!.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReturnsNotFoundCuandoProyectoNoExiste()
    {
        // Arrange
        var updateRequest = new UpdateProjectRequest("Updated Project", "Updated Description");

        // Act
        var response = await this.client.PutAsJsonAsync($"/api/projects/{Guid.NewGuid()}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReturnsNoContentCuandoProyectoExiste()
    {
        // Arrange
        var createRequest = new CreateProjectRequest("Test Project", "Test Description");
        var createResponse = await this.client.PostAsJsonAsync("/api/projects", createRequest);
        var project = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        // Act
        var response = await this.client.DeleteAsync($"/api/projects/{project!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReturnsNotFoundCuandoProyectoNoExiste()
    {
        // Act
        var response = await this.client.DeleteAsync($"/api/projects/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
